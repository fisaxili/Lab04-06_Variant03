using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab04_Variant03
{
    /// <summary>
    /// Представляет граф дорожной сети в виде списка смежности.
    /// Граф неориентированный, взвешенный.
    /// </summary>
    public class Graph
    {
        // Список смежности: вершина -> список (сосед, вес)
        private readonly Dictionary<string, List<(string neighbor, double weight)>> _adjacency;

        // Упорядоченный список вершин для стабильного вывода
        private readonly List<string> _vertices;

        public IReadOnlyList<string> Vertices => _vertices;

        public Graph()
        {
            _adjacency = new Dictionary<string, List<(string, double)>>();
            _vertices = new List<string>();
        }

        /// <summary>Добавить вершину, если её ещё нет.</summary>
        public void AddVertex(string name)
        {
            if (!_adjacency.ContainsKey(name))
            {
                _adjacency[name] = new List<(string, double)>();
                _vertices.Add(name);
            }
        }

        /// <summary>Добавить неориентированное ребро с весом.</summary>
        public void AddEdge(string from, string to, double weight)
        {
            AddVertex(from);
            AddVertex(to);
            _adjacency[from].Add((to, weight));
            _adjacency[to].Add((from, weight));
        }

        /// <summary>Получить соседей вершины.</summary>
        public IEnumerable<(string neighbor, double weight)> GetNeighbors(string vertex)
        {
            if (_adjacency.TryGetValue(vertex, out var list))
                return list;
            return Enumerable.Empty<(string, double)>();
        }
        public bool ContainsVertex(string name) => _adjacency.ContainsKey(name);

        //  BFS — обход в ширину
        /// <summary>
        /// Обход в ширину от стартовой вершины.
        /// Возвращает список вершин в порядке посещения.
        /// </summary>
        public List<string> BFS(string start)
        {
            var visited = new HashSet<string>();
            var order = new List<string>();
            var queue = new Queue<string>();

            visited.Add(start);
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                order.Add(current);

                foreach (var (neighbor, _) in GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return order;
        }

        //  DFS — обход в глубину (итеративный, через стек)
        /// <summary>
        /// Обход в глубину от стартовой вершины.
        /// Возвращает список вершин в порядке посещения.
        /// </summary>
        public List<string> DFS(string start)
        {
            var visited = new HashSet<string>();
            var order = new List<string>();
            var stack = new Stack<string>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                string current = stack.Pop();

                if (visited.Contains(current))
                    continue;

                visited.Add(current);
                order.Add(current);

                // Добавляем соседей в обратном порядке, чтобы обход шёл
                // в том же порядке, что и рекурсивный DFS
                var neighbors = GetNeighbors(current).ToList();
                for (int i = neighbors.Count - 1; i >= 0; i--)
                {
                    if (!visited.Contains(neighbors[i].neighbor))
                        stack.Push(neighbors[i].neighbor);
                }
            }

            return order;
        }
        // Достижима ли вершина B из A 

        /// <summary>
        /// Проверяет, достижима ли вершина <paramref name="target"/> из
        /// вершины <paramref name="source"/> с помощью BFS.
        /// </summary>
        public bool IsReachable(string source, string target)
        {
            if (source == target) return true;

            var visited = new HashSet<string>();
            var queue = new Queue<string>();

            visited.Add(source);
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                foreach (var (neighbor, _) in GetNeighbors(current))
                {
                    if (neighbor == target) return true;
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return false;
        }

        //  Компоненты связности
        /// <summary>
        /// Возвращает список компонент связности.
        /// Каждая компонента — список вершин, входящих в неё.
        /// </summary>
        public List<List<string>> GetConnectedComponents()
        {
            var visited = new HashSet<string>();
            var components = new List<List<string>>();

            foreach (string vertex in _vertices)
            {
                if (!visited.Contains(vertex))
                {
                    // BFS для одной компоненты
                    var component = new List<string>();
                    var queue = new Queue<string>();

                    visited.Add(vertex);
                    queue.Enqueue(vertex);

                    while (queue.Count > 0)
                    {
                        string current = queue.Dequeue();
                        component.Add(current);

                        foreach (var (neighbor, _) in GetNeighbors(current))
                        {
                            if (!visited.Contains(neighbor))
                            {
                                visited.Add(neighbor);
                                queue.Enqueue(neighbor);
                            }
                        }
                    }

                    components.Add(component);
                }
            }

            return components;
        }

        //  Алгоритм Дейкстры — кратчайшие пути от источника (ЛР 5)
        /// <summary>
        /// Алгоритм Дейкстры без использования библиотечных приоритетных очередей.
        /// Реализован через линейный поиск минимума (O(V²)).
        /// </summary>
        /// <param name="source">Начальная вершина.</param>
        /// <returns>
        /// Словарь расстояний от <paramref name="source"/> до каждой вершины
        /// и словарь предшественников для восстановления маршрута.
        /// </returns>
        public (Dictionary<string, double> dist, Dictionary<string, string?> prev)
            Dijkstra(string source)
        {
            var dist = new Dictionary<string, double>();
            var prev = new Dictionary<string, string?>();
            var unvisited = new HashSet<string>();

            // Инициализация: все расстояния = ∞, предшественник = null
            foreach (string v in _vertices)
            {
                dist[v] = double.PositiveInfinity;
                prev[v] = null;
                unvisited.Add(v);
            }
            dist[source] = 0.0;

            while (unvisited.Count > 0)
            {
                // Выбираем непосещённую вершину с минимальным расстоянием
                string? u = null;
                double minDist = double.PositiveInfinity;
                foreach (string v in unvisited)
                {
                    if (dist[v] < minDist)
                    {
                        minDist = dist[v];
                        u = v;
                    }
                }

                // Все оставшиеся вершины недостижимы
                if (u == null) break;

                unvisited.Remove(u);

                // Обновляем расстояния до соседей
                foreach (var (neighbor, weight) in GetNeighbors(u))
                {
                    if (!unvisited.Contains(neighbor)) continue;

                    double alt = dist[u] + weight;
                    if (alt < dist[neighbor])
                    {
                        dist[neighbor] = alt;
                        prev[neighbor] = u;
                    }
                }
            }

            return (dist, prev);
        }

        //  Загрузка графа из файла
        /// <summary>
        /// Загружает граф из текстового файла.
        /// Формат: секция VERTICES (по одной вершине на строку),
        /// секция EDGES (Вершина1;Вершина2;Вес).
        /// Строки, начинающиеся с '#', игнорируются.
        /// </summary>
        public static Graph LoadFromFile(string path)
        {
            var graph = new Graph();
            string section = "";

            foreach (string rawLine in File.ReadAllLines(path))
            {
                string line = rawLine.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;

                if (line == "VERTICES") { section = "V"; continue; }
                if (line == "EDGES") { section = "E"; continue; }

                if (section == "V")
                {
                    graph.AddVertex(line);
                }
                else if (section == "E")
                {
                    string[] parts = line.Split(';');
                    if (parts.Length < 3) continue;

                    string from = parts[0].Trim();
                    string to = parts[1].Trim();
                    double weight = double.Parse(parts[2].Trim(),
                                        System.Globalization.CultureInfo.InvariantCulture);
                    graph.AddEdge(from, to, weight);
                }
            }

            return graph;
        }
    }
}

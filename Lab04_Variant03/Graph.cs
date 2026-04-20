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
        /// <summary>
        /// Восстанавливает маршрут от <paramref name="source"/> до
        /// <paramref name="target"/> по таблице предшественников.
        /// Возвращает пустой список, если путь не существует.
        /// </summary>
        public static List<string> RestorePath(
            Dictionary<string, string?> prev,
            string source,
            string target)
        {
            var path = new List<string>();
            string? current = target;

            while (current != null)
            {
                path.Add(current);
                if (current == source) break;
                prev.TryGetValue(current, out current);
            }

            // Путь не найден — стартовая вершина не достигнута
            if (path.Count == 0 || path[^1] != source)
                return new List<string>();

            path.Reverse();
            return path;
        }
        // ─────────────────────────────────────────────────────────────
        //  Точки сочленения (ЛР №6, задача 1)
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Находит все точки сочленения графа алгоритмом Тарьяна (DFS).
        /// Точка сочленения — вершина, удаление которой увеличивает число
        /// компонент связности.
        /// </summary>
        public List<string> FindArticulationPoints()
        {
            var result = new HashSet<string>();
            var visited = new HashSet<string>();
            var disc = new Dictionary<string, int>();   // время входа
            var low = new Dictionary<string, int>();   // минимальное disc достижимое
            var parent = new Dictionary<string, string?>();
            int timer = 0;

            // Рекурсивный DFS через явный стек не подходит для алгоритма Тарьяна —
            // используем локальную рекурсию через вспомогательный метод
            void Dfs(string u)
            {
                visited.Add(u);
                disc[u] = low[u] = timer++;
                int childCount = 0;

                foreach (var (v, _) in GetNeighbors(u))
                {
                    if (!visited.Contains(v))
                    {
                        childCount++;
                        parent[v] = u;
                        Dfs(v);

                        // Обновляем low[u] через потомка
                        low[u] = Math.Min(low[u], low[v]);

                        // Корень дерева DFS с двумя и более детьми — точка сочленения
                        if (!parent.ContainsKey(u) && childCount > 1)
                            result.Add(u);

                        // Не корень: low[v] >= disc[u] означает, что v не может
                        // обойти u через обратное ребро
                        if (parent.ContainsKey(u) && low[v] >= disc[u])
                            result.Add(u);
                    }
                    else if (parent.TryGetValue(u, out string? p) && v != p)
                    {
                        // Обратное ребро — обновляем low[u]
                        low[u] = Math.Min(low[u], disc[v]);
                    }
                }
            }

            foreach (string v in _vertices)
            {
                if (!visited.Contains(v))
                {
                    parent.Remove(v); // корень — нет родителя
                    Dfs(v);
                }
            }

            return result.OrderBy(v => _vertices.IndexOf(v)).ToList();
        }
        // ─────────────────────────────────────────────────────────────
        //  Минимальное остовное дерево — алгоритм Прима (ЛР №6, задача 2)
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Строит минимальное остовное дерево алгоритмом Прима.
        /// Работает только для связного графа; если граф несвязный —
        /// строит МОД для компоненты, содержащей первую вершину.
        /// </summary>
        /// <returns>
        /// Список рёбер МОД в виде (from, to, weight) и суммарный вес.
        /// </returns>
        public (List<(string from, string to, double weight)> edges, double totalWeight)
            PrimMST()
        {
            if (_vertices.Count == 0)
                return (new List<(string, string, double)>(), 0);

            var inMST = new HashSet<string>();
            var mstEdges = new List<(string from, string to, double weight)>();

            // key[v]    — минимальный вес ребра, соединяющего v с деревом
            // mstParent — откуда пришли в v
            var key = new Dictionary<string, double>();
            var mstParent = new Dictionary<string, string?>();

            foreach (string v in _vertices)
            {
                key[v] = double.PositiveInfinity;
                mstParent[v] = null;
            }
            key[_vertices[0]] = 0.0;

            for (int iter = 0; iter < _vertices.Count; iter++)
            {
                // Выбираем вершину вне МОД с минимальным key
                string? u = null;
                double minKey = double.PositiveInfinity;
                foreach (string v in _vertices)
                {
                    if (!inMST.Contains(v) && key[v] < minKey)
                    {
                        minKey = key[v];
                        u = v;
                    }
                }

                if (u == null) break; // оставшиеся вершины недостижимы

                inMST.Add(u);

                // Добавляем ребро в МОД (кроме стартовой вершины)
                if (mstParent[u] != null)
                    mstEdges.Add((mstParent[u]!, u, key[u]));

                // Обновляем ключи соседей
                foreach (var (neighbor, weight) in GetNeighbors(u))
                {
                    if (!inMST.Contains(neighbor) && weight < key[neighbor])
                    {
                        key[neighbor] = weight;
                        mstParent[neighbor] = u;
                    }
                }
            }

            double total = mstEdges.Sum(e => e.weight);
            return (mstEdges, total);
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

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
    }
}
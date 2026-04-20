using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab04_Variant03;
using System.IO;

namespace Lab04_Variant03.Tests
{
    /// <summary>
    /// Модульные тесты для класса Graph (MSTest).
    /// Покрывают: структуру графа, BFS, DFS, достижимость, компоненты связности.
    /// </summary>
    [TestClass]
    public class GraphTests
    {
        // ─── Вспомогательный метод: строит простой граф для тестов ───
        //
        //   A ── B ── C
        //   |         |
        //   D ────────┘
        //
        //   E  (изолированная вершина)
        //
        private static Graph BuildSampleGraph()
        {
            var g = new Graph();
            g.AddEdge("A", "B", 1.0);
            g.AddEdge("B", "C", 2.0);
            g.AddEdge("A", "D", 3.0);
            g.AddEdge("C", "D", 1.5);
            g.AddVertex("E"); // изолированная
            return g;
        }

        // ═══════════════════════════════════════════════════════════════
        //  1. Структура графа
        // ═══════════════════════════════════════════════════════════════

        [TestMethod]
        public void AddVertex_NewVertex_IsContained()
        {
            var g = new Graph();
            g.AddVertex("X");
            Assert.IsTrue(g.ContainsVertex("X"));
        }

        [TestMethod]
        public void AddVertex_Duplicate_DoesNotDuplicate()
        {
            var g = new Graph();
            g.AddVertex("X");
            g.AddVertex("X");
            Assert.AreEqual(1, g.Vertices.Count);
        }

        [TestMethod]
        public void AddEdge_BothVerticesAdded()
        {
            var g = new Graph();
            g.AddEdge("A", "B", 5.0);
            Assert.IsTrue(g.ContainsVertex("A"));
            Assert.IsTrue(g.ContainsVertex("B"));
        }

        [TestMethod]
        public void AddEdge_GraphIsUndirected()
        {
            var g = new Graph();
            g.AddEdge("A", "B", 5.0);

            var neighborsOfA = g.GetNeighbors("A").Select(n => n.neighbor).ToList();
            var neighborsOfB = g.GetNeighbors("B").Select(n => n.neighbor).ToList();

            CollectionAssert.Contains(neighborsOfA, "B");
            CollectionAssert.Contains(neighborsOfB, "A");
        }

        [TestMethod]
        public void GetNeighbors_CorrectWeightReturned()
        {
            var g = new Graph();
            g.AddEdge("A", "B", 7.5);

            var weight = g.GetNeighbors("A").First(n => n.neighbor == "B").weight;
            Assert.AreEqual(7.5, weight);
        }

        // ═══════════════════════════════════════════════════════════════
        //  2. BFS
        // ═══════════════════════════════════════════════════════════════

        [TestMethod]
        public void BFS_VisitsAllReachableVertices()
        {
            var g = BuildSampleGraph();
            var result = g.BFS("A");

            // A, B, C, D — все связаны с A; E — изолирована
            Assert.AreEqual(4, result.Count);
            CollectionAssert.Contains(result, "A");
            CollectionAssert.Contains(result, "B");
            CollectionAssert.Contains(result, "C");
            CollectionAssert.Contains(result, "D");
        }

        [TestMethod]
        public void BFS_StartVertexIsFirst()
        {
            var g = BuildSampleGraph();
            var result = g.BFS("A");
            Assert.AreEqual("A", result[0]);
        }

        [TestMethod]
        public void BFS_IsolatedVertex_ReturnsSingleElement()
        {
            var g = BuildSampleGraph();
            var result = g.BFS("E");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("E", result[0]);
        }

        [TestMethod]
        public void BFS_NoVertexVisitedTwice()
        {
            var g = BuildSampleGraph();
            var result = g.BFS("A");
            Assert.AreEqual(result.Count, result.Distinct().Count());
        }

        // ═══════════════════════════════════════════════════════════════
        //  3. DFS
        // ═══════════════════════════════════════════════════════════════

        [TestMethod]
        public void DFS_VisitsAllReachableVertices()
        {
            var g = BuildSampleGraph();
            var result = g.DFS("A");

            Assert.AreEqual(4, result.Count);
            CollectionAssert.Contains(result, "A");
            CollectionAssert.Contains(result, "B");
            CollectionAssert.Contains(result, "C");
            CollectionAssert.Contains(result, "D");
        }

        [TestMethod]
        public void DFS_StartVertexIsFirst()
        {
            var g = BuildSampleGraph();
            var result = g.DFS("A");
            Assert.AreEqual("A", result[0]);
        }

        [TestMethod]
        public void DFS_IsolatedVertex_ReturnsSingleElement()
        {
            var g = BuildSampleGraph();
            var result = g.DFS("E");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("E", result[0]);
        }

        [TestMethod]
        public void DFS_NoVertexVisitedTwice()
        {
            var g = BuildSampleGraph();
            var result = g.DFS("A");
            Assert.AreEqual(result.Count, result.Distinct().Count());
        }

        // ═══════════════════════════════════════════════════════════════
        //  4. Достижимость
        // ═══════════════════════════════════════════════════════════════

        [TestMethod]
        public void IsReachable_ConnectedVertices_ReturnsTrue()
        {
            var g = BuildSampleGraph();
            Assert.IsTrue(g.IsReachable("A", "C"));
        }

        [TestMethod]
        public void IsReachable_SameVertex_ReturnsTrue()
        {
            var g = BuildSampleGraph();
            Assert.IsTrue(g.IsReachable("A", "A"));
        }

        [TestMethod]
        public void IsReachable_IsolatedVertex_ReturnsFalse()
        {
            var g = BuildSampleGraph();
            Assert.IsFalse(g.IsReachable("A", "E"));
        }

        [TestMethod]
        public void IsReachable_ReverseDirection_ReturnsTrue()
        {
            var g = BuildSampleGraph();
            // граф неориентированный — C достижима из A и A из C
            Assert.IsTrue(g.IsReachable("C", "A"));
        }

        // ═══════════════════════════════════════════════════════════════
        //  5. Компоненты связности
        // ═══════════════════════════════════════════════════════════════

        [TestMethod]
        public void GetConnectedComponents_TwoComponents()
        {
            var g = BuildSampleGraph();
            var components = g.GetConnectedComponents();
            // {A,B,C,D} и {E}
            Assert.AreEqual(2, components.Count);
        }

        [TestMethod]
        public void GetConnectedComponents_AllVerticesCovered()
        {
            var g = BuildSampleGraph();
            var all = g.GetConnectedComponents().SelectMany(c => c).ToList();
            Assert.AreEqual(g.Vertices.Count, all.Count);
        }

        [TestMethod]
        public void GetConnectedComponents_FullyConnected_OneComponent()
        {
            var g = new Graph();
            g.AddEdge("X", "Y", 1);
            g.AddEdge("Y", "Z", 1);
            g.AddEdge("Z", "X", 1);

            var components = g.GetConnectedComponents();
            Assert.AreEqual(1, components.Count);
        }

        [TestMethod]
        public void GetConnectedComponents_AllIsolated_EachIsOwnComponent()
        {
            var g = new Graph();
            g.AddVertex("A");
            g.AddVertex("B");
            g.AddVertex("C");

            var components = g.GetConnectedComponents();
            Assert.AreEqual(3, components.Count);
        }

        // ═══════════════════════════════════════════════════════════════
        //  6. Дейкстра (ЛР №5)
        // ═══════════════════════════════════════════════════════════════

        [TestMethod]
        public void Dijkstra_SourceDistanceIsZero()
        {
            var g = BuildSampleGraph();
            var (dist, _) = g.Dijkstra("A");
            Assert.AreEqual(0.0, dist["A"]);
        }

        [TestMethod]
        public void Dijkstra_DirectNeighborDistance_IsCorrect()
        {
            var g = BuildSampleGraph();
            var (dist, _) = g.Dijkstra("A");
            // A─B = 1.0
            Assert.AreEqual(1.0, dist["B"], 1e-9);
        }

        [TestMethod]
        public void Dijkstra_ShortestPath_ChoosesMinimum()
        {
            var g = BuildSampleGraph();
            var (dist, _) = g.Dijkstra("A");
            // A─D напрямую = 3.0, через B─C─D = 1+2+1.5 = 4.5 → должно быть 3.0
            Assert.AreEqual(3.0, dist["D"], 1e-9);
        }

        [TestMethod]
        public void Dijkstra_IsolatedVertex_IsInfinity()
        {
            var g = BuildSampleGraph();
            var (dist, _) = g.Dijkstra("A");
            Assert.AreEqual(double.PositiveInfinity, dist["E"]);
        }

        [TestMethod]
        public void Dijkstra_AllVerticesPresent_InResult()
        {
            var g = BuildSampleGraph();
            var (dist, _) = g.Dijkstra("A");
            foreach (string v in g.Vertices)
                Assert.IsTrue(dist.ContainsKey(v), $"Вершина {v} отсутствует в результате");
        }

        [TestMethod]
        public void RestorePath_ValidPath_ReturnsCorrectSequence()
        {
            var g = BuildSampleGraph();
            var (_, prev) = g.Dijkstra("A");
            var path = Graph.RestorePath(prev, "A", "C");

            // Путь должен начинаться с A и заканчиваться C
            Assert.AreEqual("A", path[0]);
            Assert.AreEqual("C", path[^1]);
        }

        [TestMethod]
        public void RestorePath_SameVertex_ReturnsSingleElement()
        {
            var g = BuildSampleGraph();
            var (_, prev) = g.Dijkstra("A");
            var path = Graph.RestorePath(prev, "A", "A");

            Assert.AreEqual(1, path.Count);
            Assert.AreEqual("A", path[0]);
        }

        [TestMethod]
        public void RestorePath_UnreachableVertex_ReturnsEmpty()
        {
            var g = BuildSampleGraph();
            var (_, prev) = g.Dijkstra("A");
            var path = Graph.RestorePath(prev, "A", "E");

            Assert.AreEqual(0, path.Count);
        }

        [TestMethod]
        public void Dijkstra_LinearGraph_DistancesAreAccumulated()
        {
            // A ─1─ B ─2─ C ─3─ D
            var g = new Graph();
            g.AddEdge("A", "B", 1.0);
            g.AddEdge("B", "C", 2.0);
            g.AddEdge("C", "D", 3.0);

            var (dist, _) = g.Dijkstra("A");

            Assert.AreEqual(1.0, dist["B"], 1e-9);
            Assert.AreEqual(3.0, dist["C"], 1e-9);
            Assert.AreEqual(6.0, dist["D"], 1e-9);
        }
    }
}


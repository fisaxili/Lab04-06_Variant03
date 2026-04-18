using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab04_Variant03;
using System.IO;

namespace Lab04_Variant03.Tests
{
    [TestClass]
    public class GraphTests
    {

        //  Вспомогательный метод: строит простой граф

        private static Graph BuildSimpleGraph()
        {
            var g = new Graph();
            g.AddEdge("A", "B", 1.0);
            g.AddEdge("B", "C", 2.0);
            g.AddEdge("B", "D", 3.0);
            g.AddVertex("E");
            return g;
        }

        //  AddVertex / AddEdge / ContainsVertex


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
        public void AddEdge_AutoCreatesVertices()
        {
            var g = new Graph();
            g.AddEdge("A", "B", 5.0);
            Assert.IsTrue(g.ContainsVertex("A"));
            Assert.IsTrue(g.ContainsVertex("B"));
        }

        [TestMethod]
        public void AddEdge_UndirectedBothDirections()
        {
            var g = new Graph();
            g.AddEdge("A", "B", 7.0);

            var neighborsA = g.GetNeighbors("A").ToList();
            var neighborsB = g.GetNeighbors("B").ToList();

            Assert.IsTrue(neighborsA.Any(n => n.neighbor == "B" && n.weight == 7.0));
            Assert.IsTrue(neighborsB.Any(n => n.neighbor == "A" && n.weight == 7.0));
        }

        [TestMethod]
        public void ContainsVertex_NonExistent_ReturnsFalse()
        {
            var g = new Graph();
            Assert.IsFalse(g.ContainsVertex("Z"));
        }

        [TestMethod]
        public void GetNeighbors_UnknownVertex_ReturnsEmpty()
        {
            var g = new Graph();
            var result = g.GetNeighbors("Unknown").ToList();
            Assert.AreEqual(0, result.Count);
        }

        //  BFS

        [TestMethod]
        public void BFS_StartsWithStartVertex()
        {
            var g = BuildSimpleGraph();
            var order = g.BFS("A");
            Assert.AreEqual("A", order[0]);
        }

        [TestMethod]
        public void BFS_VisitsAllReachableVertices()
        {
            var g = BuildSimpleGraph();
            var order = g.BFS("A");
            // A, B, C, D — все связаны; E изолирована
            CollectionAssert.Contains(order, "A");
            CollectionAssert.Contains(order, "B");
            CollectionAssert.Contains(order, "C");
            CollectionAssert.Contains(order, "D");
            CollectionAssert.DoesNotContain(order, "E");
        }

        [TestMethod]
        public void BFS_NoDuplicates()
        {
            var g = BuildSimpleGraph();
            var order = g.BFS("A");
            Assert.AreEqual(order.Count, order.Distinct().Count());
        }

        [TestMethod]
        public void BFS_SingleVertex_ReturnsSelf()
        {
            var g = new Graph();
            g.AddVertex("Solo");
            var order = g.BFS("Solo");
            Assert.AreEqual(1, order.Count);
            Assert.AreEqual("Solo", order[0]);
        }

        [TestMethod]
        public void BFS_LinearChain_CorrectOrder()
        {
            // A ─ B ─ C ─ D
            var g = new Graph();
            g.AddEdge("A", "B", 1);
            g.AddEdge("B", "C", 1);
            g.AddEdge("C", "D", 1);

            var order = g.BFS("A");
            CollectionAssert.AreEqual(new[] { "A", "B", "C", "D" }, order);
        }

        //  DFS


        [TestMethod]
        public void DFS_StartsWithStartVertex()
        {
            var g = BuildSimpleGraph();
            var order = g.DFS("A");
            Assert.AreEqual("A", order[0]);
        }

        [TestMethod]
        public void DFS_VisitsAllReachableVertices()
        {
            var g = BuildSimpleGraph();
            var order = g.DFS("A");
            CollectionAssert.Contains(order, "A");
            CollectionAssert.Contains(order, "B");
            CollectionAssert.Contains(order, "C");
            CollectionAssert.Contains(order, "D");
            CollectionAssert.DoesNotContain(order, "E");
        }

        [TestMethod]
        public void DFS_NoDuplicates()
        {
            var g = BuildSimpleGraph();
            var order = g.DFS("A");
            Assert.AreEqual(order.Count, order.Distinct().Count());
        }

        [TestMethod]
        public void DFS_SingleVertex_ReturnsSelf()
        {
            var g = new Graph();
            g.AddVertex("Solo");
            var order = g.DFS("Solo");
            Assert.AreEqual(1, order.Count);
            Assert.AreEqual("Solo", order[0]);
        }

        [TestMethod]
        public void DFS_LinearChain_CorrectOrder()
        {
            // A ─ B ─ C ─ D
            var g = new Graph();
            g.AddEdge("A", "B", 1);
            g.AddEdge("B", "C", 1);
            g.AddEdge("C", "D", 1);

            var order = g.DFS("A");
            CollectionAssert.AreEqual(new[] { "A", "B", "C", "D" }, order);
        }


        //  IsReachable


        [TestMethod]
        public void IsReachable_SameVertex_ReturnsTrue()
        {
            var g = BuildSimpleGraph();
            Assert.IsTrue(g.IsReachable("A", "A"));
        }

        [TestMethod]
        public void IsReachable_DirectNeighbor_ReturnsTrue()
        {
            var g = BuildSimpleGraph();
            Assert.IsTrue(g.IsReachable("A", "B"));
        }

        [TestMethod]
        public void IsReachable_IndirectNeighbor_ReturnsTrue()
        {
            var g = BuildSimpleGraph();
            Assert.IsTrue(g.IsReachable("A", "C"));
            Assert.IsTrue(g.IsReachable("A", "D"));
        }

        [TestMethod]
        public void IsReachable_IsolatedVertex_ReturnsFalse()
        {
            var g = BuildSimpleGraph();
            Assert.IsFalse(g.IsReachable("A", "E"));
        }

        [TestMethod]
        public void IsReachable_Undirected_BothWays()
        {
            var g = BuildSimpleGraph();
            Assert.IsTrue(g.IsReachable("C", "A"));
            Assert.IsTrue(g.IsReachable("D", "A"));
        }

        //  GetConnectedComponents


        [TestMethod]
        public void GetConnectedComponents_FullyConnected_OneComponent()
        {
            var g = new Graph();
            g.AddEdge("A", "B", 1);
            g.AddEdge("B", "C", 1);

            var components = g.GetConnectedComponents();
            Assert.AreEqual(1, components.Count);
            Assert.AreEqual(3, components[0].Count);
        }

        [TestMethod]
        public void GetConnectedComponents_TwoComponents()
        {
            var g = new Graph();
            g.AddEdge("A", "B", 1);
            g.AddEdge("C", "D", 1);

            var components = g.GetConnectedComponents();
            Assert.AreEqual(2, components.Count);
        }

        [TestMethod]
        public void GetConnectedComponents_IsolatedVertices_EachIsOwnComponent()
        {
            var g = new Graph();
            g.AddVertex("X");
            g.AddVertex("Y");
            g.AddVertex("Z");

            var components = g.GetConnectedComponents();
            Assert.AreEqual(3, components.Count);
            Assert.IsTrue(components.All(c => c.Count == 1));
        }

        [TestMethod]
        public void GetConnectedComponents_SimpleGraph_ConnectedAndIsolated()
        {
            var g = BuildSimpleGraph(); // A-B-C-D связаны, E изолирована
            var components = g.GetConnectedComponents();
            Assert.AreEqual(2, components.Count);

            var sizes = components.Select(c => c.Count).OrderByDescending(x => x).ToList();
            Assert.AreEqual(4, sizes[0]); // A, B, C, D
            Assert.AreEqual(1, sizes[1]); // E
        }

        [TestMethod]
        public void GetConnectedComponents_EmptyGraph_NoComponents()
        {
            var g = new Graph();
            var components = g.GetConnectedComponents();
            Assert.AreEqual(0, components.Count);
        }


        //  LoadFromFile


        [TestMethod]
        public void LoadFromFile_ValidFile_LoadsVerticesAndEdges()
        {
            string path = CreateTempGraphFile(
                "VERTICES",
                "A",
                "B",
                "C",
                "EDGES",
                "A;B;1.5",
                "B;C;2.0"
            );

            try
            {
                var g = Graph.LoadFromFile(path);
                Assert.AreEqual(3, g.Vertices.Count);
                Assert.IsTrue(g.ContainsVertex("A"));
                Assert.IsTrue(g.ContainsVertex("B"));
                Assert.IsTrue(g.ContainsVertex("C"));
                Assert.IsTrue(g.GetNeighbors("A").Any(n => n.neighbor == "B" && n.weight == 1.5));
                Assert.IsTrue(g.GetNeighbors("B").Any(n => n.neighbor == "C" && n.weight == 2.0));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [TestMethod]
        public void LoadFromFile_CommentsAndEmptyLines_AreIgnored()
        {
            string path = CreateTempGraphFile(
                "# Это комментарий",
                "",
                "VERTICES",
                "# ещё комментарий",
                "A",
                "B",
                "EDGES",
                "A;B;3.0"
            );

            try
            {
                var g = Graph.LoadFromFile(path);
                Assert.AreEqual(2, g.Vertices.Count);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [TestMethod]
        public void LoadFromFile_EdgeWithInvalidFormat_IsSkipped()
        {
            string path = CreateTempGraphFile(
                "VERTICES",
                "A",
                "B",
                "EDGES",
                "A;B",          // неполная строка — пропускается
                "A;B;1.0"
            );

            try
            {
                var g = Graph.LoadFromFile(path);
                // Только одно корректное ребро
                Assert.AreEqual(1, g.GetNeighbors("A").Count());
            }
            finally
            {
                File.Delete(path);
            }
        }

        [TestMethod]
        public void LoadFromFile_WeightUsesInvariantCulture()
        {
            string path = CreateTempGraphFile(
                "VERTICES",
                "A",
                "B",
                "EDGES",
                "A;B;1.75"
            );

            try
            {
                var g = Graph.LoadFromFile(path);
                double weight = g.GetNeighbors("A").First(n => n.neighbor == "B").weight;
                Assert.AreEqual(1.75, weight, 1e-9);
            }
            finally
            {
                File.Delete(path);
            }
        }


        //  Вспомогательный метод


        private static string CreateTempGraphFile(params string[] lines)
        {
            string path = Path.GetTempFileName();
            File.WriteAllLines(path, lines);
            return path;
        }
    }
}

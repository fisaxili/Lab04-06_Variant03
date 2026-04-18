using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Lab04_Variant03;

namespace Lab04_Variant03
{
    /// <summary>
    /// Главная форма приложения «Дорожная сеть района» (Вариант 3, ЛР №4).
    /// Реализует: загрузку графа, BFS, DFS, проверку достижимости, компоненты связности.
    /// </summary>
    public partial class Form1 : Form
    {
        private Graph? _graph;

        public Form1()
        {
            InitializeComponent();
        }

        //Загрузка графа

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Выберите файл графа",
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                _graph = Graph.LoadFromFile(dlg.FileName);
                PopulateComboBoxes();

                AppendOutput($"Граф загружен: {_graph.Vertices.Count} вершин.");
                AppendOutput("Вершины:");
                foreach (string v in _graph.Vertices)
                    AppendOutput($"  • {v}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла:\n{ex.Message}",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PopulateComboBoxes()
        {
            if (_graph == null) return;

            cmbBfsStart.Items.Clear();
            cmbDfsStart.Items.Clear();
            cmbReachFrom.Items.Clear();
            cmbReachTo.Items.Clear();

            foreach (string v in _graph.Vertices)
            {
                cmbBfsStart.Items.Add(v);
                cmbDfsStart.Items.Add(v);
                cmbReachFrom.Items.Add(v);
                cmbReachTo.Items.Add(v);
            }

            if (cmbBfsStart.Items.Count > 0)
            {
                cmbBfsStart.SelectedIndex = 0;
                cmbDfsStart.SelectedIndex = 0;
                cmbReachFrom.SelectedIndex = 0;
                cmbReachTo.SelectedIndex = cmbReachTo.Items.Count > 1 ? 1 : 0;
            }
        }

        // BFS

        private void btnBfs_Click(object sender, EventArgs e)
        {
            if (!CheckGraphLoaded()) return;
            if (cmbBfsStart.SelectedItem is not string start) return;

            var order = _graph!.BFS(start);

            AppendOutput("");
            AppendOutput($"═══ BFS от вершины «{start}» ═══");
            AppendOutput($"Порядок посещения ({order.Count} вершин):");
            for (int i = 0; i < order.Count; i++)
                AppendOutput($"  {i + 1}. {order[i]}");
        }

        // DFS 

        private void btnDfs_Click(object sender, EventArgs e)
        {
            if (!CheckGraphLoaded()) return;
            if (cmbDfsStart.SelectedItem is not string start) return;

            var order = _graph!.DFS(start);

            AppendOutput("");
            AppendOutput($"═══ DFS от вершины «{start}» ═══");
            AppendOutput($"Порядок посещения ({order.Count} вершин):");
            for (int i = 0; i < order.Count; i++)
                AppendOutput($"  {i + 1}. {order[i]}");
        }


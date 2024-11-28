using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace CustomLangIDE
{
    public partial class MainWindow : Window
    {
        private string currentFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        // File Operations
        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            CodeEditor.Clear();
            currentFilePath = null;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*" };
            if (openFileDialog.ShowDialog() == true)
            {
                currentFilePath = openFileDialog.FileName;
                CodeEditor.Text = File.ReadAllText(currentFilePath);
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (currentFilePath == null)
                SaveAsFile_Click(sender, e);
            else
                File.WriteAllText(currentFilePath, CodeEditor.Text);
        }

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*" };
            if (saveFileDialog.ShowDialog() == true)
            {
                currentFilePath = saveFileDialog.FileName;
                File.WriteAllText(currentFilePath, CodeEditor.Text);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Code Execution
        private void RunCode_Click(object sender, RoutedEventArgs e)
        {
            string code = CodeEditor.Text;
            try
            {
                Terminal.Clear();
                ExecuteCode(code);
            }
            catch (Exception ex)
            {
                Terminal.Text = $"Error: {ex.Message}";
            }
        }

        private void ClearOutput_Click(object sender, RoutedEventArgs e)
        {
            Terminal.Clear();
        }

        // Interpreter Logic
        private void ExecuteCode(string code)
        {
            string[] lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string output = InterpretLine(line);
                if (!string.IsNullOrWhiteSpace(output))
                    Terminal.AppendText(output + Environment.NewLine);
            }
        }

        private string InterpretLine(string line)
        {
            // Example token parsing logic
            if (line.StartsWith("gyatt")) // Variable declaration
            {
                return $"Variable declared: {line.Substring(6)}";
            }
            else if (line.StartsWith("drake")) // Output
            {
                return line.Substring(6);
            }

            return $"Unknown command: {line}";
        }
    }
}

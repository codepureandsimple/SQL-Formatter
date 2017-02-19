using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SqlFormatter {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        HashSet<string> _keywords = new HashSet<string>();
        public MainWindow() {
            InitializeComponent();
        }

        private void LoadKeywords() {
            try {
                string directoryPath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                string[] keywords = null;
                do {
                    var fileName = System.IO.Path.Combine(directoryPath, "keywords.json");
                    if (File.Exists(fileName)) {
                        keywords = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(fileName));
                        directoryPath = null;
                    }
                    else {
                        DirectoryInfo di = Directory.GetParent(directoryPath);
                        if (di == null) {
                            directoryPath = null;
                        }
                        else {
                            directoryPath = di.FullName;
                        }
                    }

                } while (directoryPath != null);

                if (keywords != null && keywords.Length > 0) {
                    foreach (var keyword in keywords) {
                        _keywords.Add(keyword);
                    }
                }
            }
            catch(Exception e) {
                MessageBox.Show($"An unexpected error occurred while trying to read keywords.json file. {e.Message}", "Error");
            }
        }

        private void formatSqlButton_Click(object sender, RoutedEventArgs e) {
            // TODO: just do this on load? This is great for testing for now...
            LoadKeywords();

            // TODO: none of this is optimized yet, all runs on the UI thread, and is generally not anything to be proud of

            // Break input lines into a list
            List<string> lines = this.input.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            // Iterate and perform fixes
            for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++) {

                // Fix leading commas 
                if(lineIndex > 0) {
                    if(lines[lineIndex].Trim().StartsWith(",")) {
                        int indexOfComma = lines[lineIndex].IndexOf(',');
                        lines[lineIndex] = lines[lineIndex].Remove(lines[lineIndex].IndexOf(','), 1);
                        lines[lineIndex - 1] = string.Concat(lines[lineIndex - 1], ",");
                    }
                }

                // Lowercase keywords
                foreach(var keyword in _keywords) {
                    //var regex = new Regex($"\\b{Regex.Escape(keyword)}\\b", RegexOptions.IgnoreCase);
                    var regex = new Regex($"\\b(?<!\\[){Regex.Escape(keyword)}(?!])\\b", RegexOptions.IgnoreCase);
                    Match m = regex.Match(lines[lineIndex]);
                    if(m.Success) {
                        lines[lineIndex] = lines[lineIndex].Remove(m.Index, m.Length).Insert(m.Index, keyword.ToLowerInvariant());
                    }
                }
            }

            this.output.Clear();
            this.output.AppendText(string.Join(Environment.NewLine, lines));
        }
    }
}

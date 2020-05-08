using Chart.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Chart
{
    public partial class Chart : Form
    {
        private readonly string _fullPath = "..\\..\\CSVs\\Student.csv";

        private List<Student> students = new List<Student>();

        private List<string> label = new List<string> { "Math Point", "Physical Point", "Chemistry Point" };

        private Series _series;

        public Chart()
        {
            InitializeComponent();
        }

        private void Chart_Load(object sender, EventArgs e)
        {
            students = _getStudents(_fullPath);

            student_combobox.DropDownStyle = ComboBoxStyle.DropDown;
            student_combobox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            student_combobox.AutoCompleteSource = AutoCompleteSource.ListItems;
            student_combobox.ValueMember = "Id";
            student_combobox.DisplayMember = "Name";

            students.ForEach(st =>student_combobox.Items.Add(st));

            student_chart.Series.Clear();
            label.ForEach(str => {
                _series = student_chart.Series.Add(str);
                _series.Points.Add(0);
            });
        }

        private List<Student> _getStudents(string csvPath)
        {
            List<string[]> rows = File.ReadAllLines(csvPath).Select(x => x.Split(',')).ToList();
            List<Student> students = new List<Student>();

            int rowLength = rows.Count();
            for(int i = 1; i < rowLength; i++)
            {
                string[] vals = rows[i];

                Student student = new Student();

                student.Id = int.Parse(vals[0]);
                student.Name = vals[2] + vals[3];
                student.Age = DateTime.Now.Year - DateTime.Parse(vals[12]).Year;
                student.MathPoint = int.Parse(vals[14]);
                student.PhysicalPoint = int.Parse(vals[15]);
                student.ChemistryPoint = int.Parse(vals[16]);

                students.Add(student);
            }

            return students;
        }

        private void student_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            Student selectedStudent = (Student) comboBox.SelectedItem;

            Student student = students.Where(st => st.Id == selectedStudent.Id).ToArray()[0];

            Draw(student);
        }

        private void Draw(Student student = null)
        {
            student_chart.Series["Math Point"].Points.Clear();
            student_chart.Series["Math Point"].Points.Add(student.MathPoint);

            student_chart.Series["Physical Point"].Points.Clear();
            student_chart.Series["Physical Point"].Points.Add(student.PhysicalPoint);

            student_chart.Series["Chemistry Point"].Points.Clear();
            student_chart.Series["Chemistry Point"].Points.Add(student.ChemistryPoint);
        }
    }
}

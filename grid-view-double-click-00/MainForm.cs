using System.ComponentModel;

namespace grid_view_double_click_00
{
    // https://docs.devexpress.com/WindowsForms/634/controls-and-libraries/data-grid/data-binding
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            gridControl.DataSource = Animals;
            Animals.Add(new Animal { Name = "Luna", Kind = Kind.Cat });
            Animals.Add(new Animal { Name = "Daisy", Kind = Kind.Dog});
        }
        public BindingList<Animal> Animals { get; } = new BindingList<Animal>();
    }
    public enum Kind
    {
        Other,
        Cat,
        Dog,
    }
    public class Animal
    {
        public string ID { get; set; } = Guid.NewGuid().ToString().Substring(0,8);
        public string? Name { get; set; }
        public Kind Kind { get; set; }
    }
}
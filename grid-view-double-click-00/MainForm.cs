using System.ComponentModel;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;
using System.ComponentModel.DataAnnotations;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace grid_view_double_click_00
{
    // https://docs.devexpress.com/WindowsForms/634/controls-and-libraries/data-grid/data-binding
    // https://supportcenter.devexpress.com/ticket/details/q303121/gridcontrol-disable-edit-on-click-enable-edit-on-double-click
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            gridControl.DataSource = Animals;
            Animals.Add(new Animal { Name = "Luna", Kind = Kind.Cat });
            Animals.Add(new Animal { Name = "Daisy", Kind = Kind.Dog});
            gridControl.DoubleClick += onGridControlDoubleClick;
            var view = (GridView)gridControl.MainView;
            view.OptionsBehavior.Editable = false;
        }
        private void onGridControlDoubleClick(object? sender, EventArgs e)
        {
            if ((sender is GridControl control) && e is DXMouseEventArgs args)
            {
                var hittest = (GridHitInfo)control.MainView.CalcHitInfo(args.Location);

                // Set title bar text
                Text = $"DoubleClick on row: {hittest.RowHandle}, column: {hittest.Column.GetCaption()}";

                // BTW don't block the click event to do this.
                BeginInvoke(() =>
                    MessageBox.Show(Animals[hittest.RowHandle].ToString())
                );
            }
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
        [Display(AutoGenerateField = false)]
        public string ID { get; set; } = Guid.NewGuid().ToString().Substring(0,8);
        public string? Name { get; set; }
        public Kind Kind { get; set; }
        public override string ToString() => $"{Name}, {Kind}";
    }
}
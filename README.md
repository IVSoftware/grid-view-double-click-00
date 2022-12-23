Your question is **How to get value of Row Double-Click Row in GridView**. I [reproduced]() your issue using a minimal form and I believe the problem is coming from the [as](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/type-testing-and-cast#as-operator) operator. When I replaced the call with an explicit cast it seems to work.

    private void gridView2_DoubleClick_1(object? sender, EventArgs e)
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

    ![screenshot]()
    ***
    [Minimal Reproducible Example](https://stackoverflow.com/help/minimal-reproducible-example)

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


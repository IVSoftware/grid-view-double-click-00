Your question is **How to get value of Row Double-Click Row in GridView** (DevExpress). 

Referencing `DevExpress.Win.Design 22.1.6` NuGet I was able to reproduce this issue using a minimal form and I believe the problem is coming from `sender as GridView` which seems to evaluate null (because the sender `is GridControl` and evidently is not a compatible reference per the [as](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/type-testing-and-cast#as-operator) operator documentation). 

When I cast the objects correctly, everything seems to work.

    private void gridView2_DoubleClick_1(object? sender, EventArgs e)
    {
        if (
               (sender is GridControl control) &&
               (control.MainView is GridView gridView) &&
               (e is DXMouseEventArgs args))
        {
            var hittest = gridView.CalcHitInfo(args.Location);

            // BTW don't block the double-click event to do this.
            BeginInvoke(() =>
            {
                MessageBox.Show(
                    text: Animals[hittest.RowHandle].ToString(),
                    caption: $"DoubleClick on row: {hittest.RowHandle}, column: {hittest.Column.GetCaption()}"
                );
            });
        }
    }

[![double-click response][1]][1]

***
My [minimal reproducible example](https://stackoverflow.com/help/minimal-reproducible-example) uses this code to set up the `DevExpress.XtraGrid.GridControl`.

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
            view.Appearance.FocusedCell.BackColor = Color.CadetBlue;
            view.Appearance.FocusedCell.ForeColor = Color.White;
        }
        private void onGridControlDoubleClick(object? sender, EventArgs e)
        {
            if (
                   (sender is GridControl control) &&
                   (control.MainView is GridView gridView) &&
                   (e is DXMouseEventArgs args))
            {
                var hittest = gridView.CalcHitInfo(args.Location);

                // BTW don't block the double-click event to do this.
                BeginInvoke(() =>
                {
                    MessageBox.Show(
                       text: Animals[hittest.RowHandle].ToString(),
                       caption: $"DoubleClick on row: {hittest.RowHandle}, column: {hittest.Column.GetCaption()}"
                    );
                });
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
        public override string ToString() => $"{Name} ({Kind})";
    }
    
  [1]: https://i.stack.imgur.com/sD5Vw.png

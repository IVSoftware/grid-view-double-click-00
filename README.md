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


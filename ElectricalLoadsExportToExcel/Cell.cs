namespace ElectricalLoadsExportToExcel
{
    public class Cell
    {
        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Cell(Cell cell)
        {
            Row = cell.Row;
            Column = cell.Column;
        }

        public int Row { get; set; }
        public int Column { get; set; }

        public void NextRow()
        {
            Row++;
        }

        public void NextColumn()
        {
            Column++;
        }

        public override string ToString()
        {
            var col = (byte)'A' - 1 + Column;
            return (char)col + Row.ToString();
        }
    }
}

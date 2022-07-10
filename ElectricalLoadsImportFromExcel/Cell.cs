namespace ElectricalLoadsImportToExcel
{
    public class Cell
    {
        public Cell(int row, int colomn)
        {
            Row = row;
            Colomn = colomn;
        }
        public Cell(Cell cell)
        {
            Row = cell.Row;
            Colomn = cell.Colomn;
        }

        public int Row { get; set; }
        public int Colomn{get;set;}

        public void NextRow()
        {
            Row++;
        }

        public void NextColomn()
        {
            Colomn++;
        }

        public override string ToString()
        {
            var col = (byte)'A'-1 + Colomn;
            return (char)col +Row.ToString();
        }
    }
}

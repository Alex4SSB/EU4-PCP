namespace EU4_PCP_WPF.Models
{
    public class TableProvince
	{
		public string B_Color { get; set; }
		public int ID { get; set; }
		public string P_Name { get; set; }
		public byte Red { get; set; }
		public byte Green { get; set; }
		public byte Blue { get; set; }

		public TableProvince(Province prov)
        {
			ID = prov.Index;
			P_Name = prov.ToString();
			Red = prov.Color.R;
			Green = prov.Color.G;
			Blue = prov.Color.B;

			B_Color = '#' + prov.Color.Color.Name;
        }

        public static explicit operator TableProvince(Province v)
        {
			return new TableProvince(v);
        }
    }
}

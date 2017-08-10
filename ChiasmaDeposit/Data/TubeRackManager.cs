using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{

    public enum TubeRackForTagSourceParsingTable
    {
        TubeRackName = 0,
        Position = 1,
        TagGroup = 2,
        TagIndex = 3
    }

    public class TubeRackManager : ChiasmaDepData
    {

        private enum SlotForTagSourceTableColumns
        {
            Id = 0,
            TubeRackId = 1,
            PositionX = 2,
            PositionY = 3,
            TagIndexId = 4
        }

        private TubeRackManager()
            : base()
        { }

        public static DataTable GetTubeRackForTagSourceParsingTable()
        {
            DataColumn column;
            DataTable table = new DataTable();

            column = new DataColumn(TubeRackForTagSourceParsingTable.TubeRackName.ToString(), typeof(string));
            column.AllowDBNull = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn(TubeRackForTagSourceParsingTable.Position.ToString(), typeof(string));
            column.AllowDBNull = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn(TubeRackForTagSourceParsingTable.TagGroup.ToString(), typeof(string));
            column.AllowDBNull = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn(TubeRackForTagSourceParsingTable.TagIndex.ToString(), typeof(string));
            column.AllowDBNull = false;
            column.Unique = false;
            table.Columns.Add(column);

            return table;
        }

        public static bool ParseSlotPosition(string position, out int row0based, out int col0based)
        {
            row0based = position.ToUpper()[0] - 'A';
            if (!int.TryParse(position.Substring(1), out col0based))
            {
                return false;
            }
            col0based--;
            return true;
        }
    }
}

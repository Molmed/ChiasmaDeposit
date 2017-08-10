using System;
using Molmed.ChiasmaDep.Database;

namespace Molmed.ChiasmaDep.Data
{
    public interface IDataComment
    {
        String GetComment();
        Boolean HasComment();
    }

    public abstract class DataComment : DataIdentity, IDataComment
    {
        private String MyComment;

        public DataComment(DataReader dataReader)
            : base(dataReader)
        {
            MyComment = dataReader.GetString(DataCommentData.COMMENT);
        }

        public String GetComment()
        {
            return MyComment;
        }

        public Boolean HasComment()
        {
            return IsNotEmpty(MyComment);
        }
    }
}

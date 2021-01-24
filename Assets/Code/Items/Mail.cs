using Pokemon;

namespace Items
{
    #region Structs

    public struct Mail
    {
        public Items item;
        public string message;
        public string sender;
        public Species[] species;
        public int[] form_ids;
    }

    #endregion

}
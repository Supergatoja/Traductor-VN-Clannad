namespace SeenExtractor.Data.Models.Dialogos.Search
{
    public class SearchDialogModel
    {
        public List<SearchDialogResult> Searches { get; set; }
        private int currentItem = 0;
        public int CurrentItem
        {
            get => currentItem;
            set
            {
                currentItem = value;
                Continua(currentItem);
            }
        }
        public bool Next { get; set; }
        public bool Before { get; set; }
        public void Continua(int value)
        {
            Next = currentItem < Searches.Count - 1 ? true : false;
            Before = currentItem > 0 ? true : false;
        }

        internal int GetMatch()
        {
            return Searches[CurrentItem].Line - 1;
        }
    }

    public class SearchDialogResult
    {
        public int Line { get; set; }
        public string? Character { get; set; }
        public string? Dialog { get; set; }
    }

}

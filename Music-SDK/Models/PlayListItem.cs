using System;
using System.Collections.Generic;
using System.Text;

namespace Music.SDK.Models
{
    public class PlayListItem
    {
        public long PlayListId { get; set; }

        public string PlayListName { get; set; }

        public string PlayListDesc { get; set; }

        public string PlayListAuthor { get; set; }

        public List<SongItem> Songs { get; set; }
    }
}

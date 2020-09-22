using SolrNet.Attributes;
using System;
using System.Collections.Generic;

namespace SolrDemo.Common
{
    public class Post
    {
        [SolrUniqueKey("id")]
        public string Id { get; set; }

        [SolrField("postTypeId")]
        public int PostTypeId { get; set; }

        [SolrField("title")]
        public string Title { get; set; }

        [SolrField("body")]
        public string Body { get; set; }

        [SolrField("tags")]
        public ICollection<string> Tags { get; set; } = new List<string>();

        [SolrField("postScore")]
        public float PostScore { get; set; }

        [SolrField("ownerUserId")]
        public int? OwnerUserId { get; set; }

        [SolrField("answerCount")]
        public int? AnswerCount { get; set; }

        [SolrField("commentCount")]
        public int CommentCount { get; set; }

        [SolrField("favoriteCount")]
        public int? FavoriteCount { get; set; }

        [SolrField("viewCount")]
        public int? ViewCount { get; set; }

        [SolrField("creationDate")]
        public DateTime CreationDate { get; set; }

        [SolrField("lastActivityDate")]
        public DateTime LastActivityDate { get; set; }

        [SolrField("closedDate")]
        public DateTime? ClosedDate { get; set; }
    }
}

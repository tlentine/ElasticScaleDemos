﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See MicrosoftLICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DdrDemo {
    public class Blog {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BlogId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

    public class Post {
        public int PostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }

    public class User {
        [Key]
        public string UserName { get; set; }

        public string DisplayName { get; set; }
    }
}

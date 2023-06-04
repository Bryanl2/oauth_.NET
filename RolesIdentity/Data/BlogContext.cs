﻿using Microsoft.EntityFrameworkCore;

namespace RolesIdentity.Data
{
    public class BlogContext:DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options):base(options)
        {

        }
        public DbSet<Blog> Blogs { get; set; }
    }
}

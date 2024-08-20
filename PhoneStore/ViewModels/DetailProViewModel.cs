using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Models;

namespace ToyStore.ViewModels
{
    public class DetailProViewModel
    {
        public IEnumerable<SanPham> Detail { get; set; }
    }
}
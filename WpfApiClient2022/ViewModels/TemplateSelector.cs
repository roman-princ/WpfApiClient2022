using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApiClient2022.Models;
using static WpfApiClient2022.Models.ActorsJson;
using static WpfApiClient2022.Models.MoviesJson;

namespace WpfApiClient2022.ViewModels
{
    internal class TemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ActorObject)
            {
                return ActorTemplate;
            }
            if (item is MovieObject)
            {
                return MovieTemplate;
            }
            return base.SelectTemplate(item, container);

        }

        public DataTemplate MovieTemplate { get; set; }
        public DataTemplate ActorTemplate { get; set; }
    }
}

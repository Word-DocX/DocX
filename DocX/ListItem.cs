using System.Xml.Linq;

namespace Novacode
{
  public class ListItem
  {
    public ListItemType ListItemType { get; set; }
    public int Level { get; set; }
    public string ListItemText { get; set; }
    public int Start { get; set; }

  }

}
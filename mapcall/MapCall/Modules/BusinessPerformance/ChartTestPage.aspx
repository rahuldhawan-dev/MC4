<%@ Page Language="C#" Description="dotnetCHARTING Component"%>
<%@ Import Namespace="System.Drawing" %>
<%@ Register TagPrefix="dotnet" Namespace="dotnetCHARTING" Assembly="dotnetCHARTING"%>


<script runat="server">

SeriesCollection getRandomData()
{
      var SC = new SeriesCollection();
      var myR = new Random();
      for(var a = 0; a < 4; a++)
      {
            var s = new Series();
            s.Name = "Series " + a;
            for(var b = 0; b < 5; b++)
            {
                  var e = new Element();
                  e.Name = "E " + b;
                  e.YValue = myR.Next(50);
                  s.Elements.Add(e);
            }
            SC.Add(s);
      }
// Set Different Colors for our Series
      SC[0].DefaultElement.Color = Color.FromArgb(49,255,49);
      SC[1].DefaultElement.Color = Color.FromArgb(255,255,0);
      SC[2].DefaultElement.Color = Color.FromArgb(255,99,49);
      SC[3].DefaultElement.Color = Color.FromArgb(0,156,255);
      
      return SC;
}


void Page_Load(Object sender,EventArgs e)
{
    // referring to "Chart", instead of this.Chart, 
    // gives "Cannot access non-static property 'x' in static contact" 
    // errors.
    
    var c = this.Chart;
    
      // Set the title.
      c.Title="My Chart";

      // Set the Depth
      c.Depth = 15;

      // Set 3D
      c.Use3D = false;

      // set the x axis clustering
      c.XAxis.ClusterColumns = false;

      // Set a default transparency
      c.DefaultSeries.DefaultElement.Transparency = 20;

      // Set the Default Series Type
      c.DefaultSeries.Type = SeriesType.Line;

      // Set the y Axis Scale
      c.YAxis.Scale = Scale.Normal;

      // Set the x axis label
      c.XAxis.Label.Text="X Axis Lolbal";

      // Set the y axis label
      c.YAxis.Label.Text="Y Axis Label";

      // Set the directory where the images will be stored.
      c.TempDirectory="temp";


      // Set he chart size.
      c.Width = 600;
      c.Height = 350;

      // Add the random data.
      c.SeriesCollection.Add(getRandomData());


}
</script>
<html xmlns="http://www.w3.org/1999/xhtml"><head><title>Gallery Sample</title></head>
<body>
<div style="text-align:center">
<dotnet:Chart id="Chart" runat="server"/>
</div>
</body>
</html>
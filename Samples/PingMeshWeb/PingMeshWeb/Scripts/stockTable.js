$(document).ready(function () {

    //********************************************
    // function called to create sparkline for each row.  Stock symbol is passed in from table
    //********************************************
    function renderSparkline(_symbol) {

        var that = this;  //you lose 'this' (aka - sparkline column object) in the async .getJSON call below and therefore need to capture the state 

        $.getJSON('/api/stocksvalue/'
               + _symbol
                 , function (chartData) {
                     console.log(chartData);

                     // GET STOCK TICKER STRING - This function will extract the current stock from the data-set.  We'll use this to create unique element IDs for the SVG and axis.
                     var stockTicker = d3.max(chartData, function (d) { return d.StockID });

                     // DEFINE CANVAS SIZE - Set the dimensions of the canvas where you'll plat the axis an line
                     var margin = { top: 0, right: 0, bottom: 0, left: 0 },
                         width = 250 - margin.left - margin.right,
                         height = 16 - margin.top - margin.bottom,
                         barPadding = 0;

                     // DEFINE BOUNDARIES OF VERTICAL SCALE  - These are the max and min Y values which will be used to set the domain for heatmap colors
                     var minYRange = d3.min(chartData, function (d) { return d.ValueClose; }),
                         maxYRange = d3.max(chartData, function (d) { return d.ValueClose; });

                     // FORCE SCALE RANGE TO MATCH SELECTED TIME RANGE - This appends data points that bind the start and end time parameters to the X-Scale
                     var datumStatTime =
                          {
                              StockID: stockTicker
                            , Date: startDate
                            , ValueClose: 0
                          }

                     chartData.push(datumStatTime);

                     var datumEndTime =
                          {
                              StockID: stockTicker
                           , Date: endDate
                           , ValueClose: 0
                          }

                     chartData.push(datumEndTime);

                     // DEFINE SCALES - Set the scales that will be used to size the axis
                     var x = d3.time.scale()
                         .range([0, width])
                         .domain(d3.extent(chartData, function (d) { return new Date(d.Date); }));

                     var y = d3.scale.linear()
                         .range([height, 0])
                         .domain([0, d3.max(chartData, function (d) { return d.ValueClose; })]);

                     // FUNCTION FOR AXIS GENERATION - These functions create the axis objects
                     var xAxisGen = d3.svg.axis().scale(x)
                         .orient("bottom").ticks(5);

                     var yAxisGen = d3.svg.axis().scale(y)
                         .orient("left").ticks(5);

                     // CREATE CANVAS - Adds the svg canvas to the DIV tag in the HTML (i.e. - "<div id="stockChart"></div>") - remember to prefix IDs with the "#" symbol when accessing.
                     var svg = d3.select(that)
                         .append("svg")
                             .attr("width", width + margin.left + margin.right)
                             .attr("height", height + margin.top + margin.bottom)
                             .attr("id", "d3-svg-" + stockTicker)  //Add an ID attribute with ticket symbol so you can select these objects in the future
                         .append("g")
                             .attr("transform",
                                   "translate(" + margin.left + "," + margin.top + ")");

                     // ADD BARS 
                     svg.selectAll(".bar")
                         .data(chartData)
                       .enter().append("rect")
                         .style("fill", "blue")
                         .attr("x", function (d) { return x(new Date(d.Date)); })
                         .attr("y", function (d) { return y(d.ValueClose); })
                         .attr("width", function (d) { return width / chartData.length })
                         .attr("height", function (d) { return height - y(d.ValueClose); });
                 });
    }


    //********************************************
    //hardcoded data (for now)
    //********************************************
    var columns = ['Stock', 'Symbol']
    var data = [
          ['Microsoft', 'MSFT']
        , ['Apple', 'AAPL']
        , ['SPDR S&P 500', 'SPY']
        , ['Gold ETF', 'GLD']
        , ['Tesla', 'TSLA']
        , ['Solar City', 'SCTY']
    ]

    //variables
    var startDate = document.getElementById('date-start').value;
    var endDate = document.getElementById('date-end').value;

    //********************************************
    // create table
    //********************************************
    var table = d3.select("#div-stocktable")
        .append("table")
        .attr("class", "table table-striped table-bordered table-condensed")
    ;

    var thead = table
               .append("thead")
               .append("tr");

    thead.selectAll("th")
        .data(columns)
        .enter()
        .append("th")
        .text(function (d) { return d; });

    var tbody = table.append("tbody");

    var trows = tbody
        .selectAll("tr")
        .data(data)
        .enter()
        .append("tr")
        .attr("style", "padding:0px; height: 15px");

    var tcells = trows
        .selectAll("td")
        .data(function (d, i) { return d; })
        .enter()
        .append("td")
        .text(function (d, i) { return d; })
        .attr("style", "height: 15px ; padding:0px ");

    // add a column for sparkline and render graphs for each row via the renderSparkline function call
    thead.append("th")
        .text('Trend');

    thead.selectAll("th")
      .attr("style", 'background-color:darkgray; padding:0px; height: 15px');

    //Select each Sparkline columns and create sparkline graphs using the .enter() method
    trows.selectAll("td.sparkline")
              .data(function (d) { return [d[1]]; })  //passes the symbol from the array to the each function
              .enter()
                .append("td")
                .attr("class", "sparklinegraph")
                .attr("style", 'padding:0px; height: 0px ; width: "' + 0 + '"')
                .each(renderSparkline);

});






(function () {

    $(function () {
            $(".button-collapse").sideNav();
            $('.datepicker').pickadate();
            $('[data-toggle="tooltip"]').tooltip();
            var data = {
                labels: ["January", "February", "March", "April", "May", "June", "July"],
                datasets: [{
                    label: "My First dataset",
                    fillColor: "rgba(220,220,220,0.2)",
                    strokeColor: "rgba(220,220,220,1)",
                    pointColor: "rgba(220,220,220,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(0,0,0,.15)",
                    data: [65, 59, 80, 81, 56, 55, 40],
                    backgroundColor: "#4CAF50",
                }, {
                    label: "My Second dataset",
                    fillColor: "rgba(255,255,255,.25)",
                    strokeColor: "rgba(255,255,255,.75)",
                    pointColor: "#fff",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(0,0,0,.15)",
                    data: [28, 48, 40, 19, 86, 27, 90]
                }]
            };


            var dataPie = [{
                value: 300,
                color: "#4caf50",
                highlight: "#66bb6a",
                label: "Google Chrome"
            }, {
                value: 50,
                color: "#03a9f4",
                highlight: "#29b6f6",
                label: "Edge"
            }, {
                value: 100,
                color: "#d32f2f",
                highlight: "#e53935",
                label: "Firefox"
            }]

            var option = {
                responsive: true,
                // set font color
                scaleFontColor: "#fff",
                // font family
                defaultFontFamily: "'Roboto', sans-serif",
                // background grid lines color
                scaleGridLineColor: "rgba(255,255,255,.1)",
                // hide vertical lines
                scaleShowVerticalLines: false,
            };

            // // Get the context of the canvas element we want to select
            // var ctx = document.getElementById("sales").getContext('2d');
            // var myLineChart = new Chart(ctx).Line(data, option); //'Line' defines type of the chart.

            // // Get the context of the canvas element we want to select
            // var ctx = document.getElementById("conversion").getContext('2d');
            // var myRadarChart = new Chart(ctx).Radar(data, option);

            // Get the context of the canvas element we want to select

            //bar
            var ctxB = document.getElementById("traffic").getContext('2d');
            var myBarChart = new Chart(ctxB, {
                type: 'bar',
                data: {
                    labels: ["January", "Febuary", "March", "April", "May", "June"],
                    datasets: [{
                        label: '# of Votes',
                        data: [12, 19, 3, 5, 2, 3],
                        backgroundColor: [
                            'rgba(255, 255, 255, 0.3)',
                            'rgba(255, 255, 255, 0.3)',
                            'rgba(255, 255, 255, 0.3)',
                            'rgba(255, 255, 255, 0.3)',
                            'rgba(255, 255, 255, 0.3)',
                            'rgba(255, 255, 255, 0.3)'
                        ],
                        borderColor: [
                            'rgba(255, 255, 255, 1)',
                            'rgba(255, 255, 255, 1)',
                            'rgba(255, 255, 255, 1)',
                            'rgba(255, 255, 255, 1)',
                            'rgba(255, 255, 255, 1)',
                            'rgba(255, 255, 255, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    legend: {
                        labels: {
                            fontColor: "white"
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontColor: "white"
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                fontColor: "white"
                            }
                        }]
                    }
                }
            });

            //pie
            var ctxP = document.getElementById("doughnutChart").getContext('2d');
            var myPieChart = new Chart(ctxP, {
                type: 'doughnut',
                data: {
                    labels: ["March", "April", "May", "June"],
                    datasets: [{
                        data: [160, 50, 80, 60],
                        backgroundColor: ["#4285F4", "#ffbb33", "#29b6f6", "#FF5252"],
                        hoverBackgroundColor: ["#6ea0f2", "#fec451", "#52c3f6", "#fa6e6e"]
                    }]
                },
                options: {
                    responsive: true
                }
            });
    });

  
})();
var barChart, pieChart;

function drawCharts(labels, dataTongQS, dataQsVang) {
    var barCtx = document.getElementById('tkchart').getContext('2d');
    var pieCtx = document.getElementById('piechart').getContext('2d');

    if (barChart) {
        barChart.destroy();
    }
    if (pieChart) {
        pieChart.destroy();
    }
    var barChartData = {
        labels: labels,
        datasets: [
            {
                label: 'Tổng quân số',
                data: dataTongQS,
                backgroundColor: 'rgba(0,119,203,0.3)',
                borderColor: 'rgba(0,119,204,1)',
                borderWidth: 1
            },
            {
                label: 'Quân số vắng',
                data: dataQsVang,
                backgroundColor: 'rgba(255,0,0,0.3)',
                borderColor: 'rgba(255,0,0,1)',
                borderWidth: 1
            }
        ]
    };

    barChart = new Chart(barCtx, {
        type: 'bar',
        data: barChartData,
        options: {
            scales: {
                xAxes: [{
                    stacked: true
                }],
                yAxes: [{
                    stacked: false,
                    ticks: {
                        beginAtZero: true,
                        callback: function (value) { return Number(value).toLocaleString(); }
                    }
                }]
            },
            responsive: true,
            legend: {
                position: 'top',
                align: 'start'
            }
        }
    });

    var totalTongQS = dataTongQS.reduce((acc, val) => acc + val, 0);
    var totalQsVang = dataQsVang.reduce((acc, val) => acc + val, 0);
    var pieData = [totalTongQS, totalQsVang];
    var pieLabels = ['Tổng quân số', 'Quân số vắng'];

    pieChart = new Chart(pieCtx, {
        type: 'doughnut',
        data: {
            labels: pieLabels,
            datasets: [{
                data: pieData,
                backgroundColor: ['rgba(0,119,203,0.3)', 'rgba(255,0,0,0.3)'],
                borderColor: ['rgba(0,119,204,1)', 'rgba(255,0,0,1)'],
                hoverBackgroundColor: ['#2e59d9', '#17a673'],
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }],
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
            },
            legend: {
                display: false
            },
            cutoutPercentage: 80,
        },
    });
}

function getDataForChart() {
    var startDate = document.getElementById('startDatePicker').value;
    var endDate = document.getElementById('endDatePicker').value;
    var idDv = document.getElementById('idDvDropdown').value;

    $.ajax({
        type: 'GET',
        url: '/ThongKeRecord/UpdateChart',
        data: { startDate: startDate, endDate: endDate, idDv: idDv },
        success: function (chartData) {
            if (chartData && chartData.labels && chartData.dataTongQS && chartData.dataQsVang) {
                drawCharts(chartData.labels, chartData.dataTongQS, chartData.dataQsVang);
            } else {
                alert('Dữ liệu không hợp lệ.');
            }
        },
        error: function () {
            alert('Có lỗi xảy ra khi lấy dữ liệu từ máy chủ.');
        }
    });
}

function getDataForChartHV() {
    var startDate = document.getElementById('startDatePicker').value;
    var endDate = document.getElementById('endDatePicker').value;

    $.ajax({
        type: 'GET',
        url: '/ThongKeRecord/UpdateChartForHocVien',
        data: { startDate: startDate, endDate: endDate },
        success: function (chartData) {
            if (chartData && chartData.labels && chartData.dataTongQS && chartData.dataQsVang) {
                drawCharts(chartData.labels, chartData.dataTongQS, chartData.dataQsVang);
            } else {
                alert('Dữ liệu không hợp lệ.');
            }
        },
        error: function () {
            alert('Có lỗi xảy ra khi lấy dữ liệu từ máy chủ.');
        }
    });
}




/*var chart;

function drawChart(labels, dataTongQS, dataQsVang) {
    var ctx = document.getElementById('tkchart').getContext('2d');
    if (chart) {
        chart.destroy();
    }

    var chartData = {
        labels: labels,
        datasets: [
            {
                label: 'Tổng quân số',
                data: dataTongQS,
                backgroundColor: 'rgba(0,119,203,0.3)',
                borderColor: 'rgba(0,119,204,1)',
                borderWidth: 1
            },
            {
                label: 'Quân số vắng',
                data: dataQsVang,
                backgroundColor: 'rgba(255,0,0,0.3)',
                borderColor: 'rgba(255,0,0,1)',
                borderWidth: 1
            }
        ]
    };

    chart = new Chart(ctx, {
        type: 'bar',
        data: chartData,
        options: {
            scales: {
                xAxes: [{
                    stacked: true
                }],
                yAxes: [{
                    stacked: false,
                    ticks: {
                        beginAtZero: true,
                        callback: function (value) { return Number(value).toLocaleString(); }
                    }
                }]
            },
            responsive: true,
            legend: {
                position: 'top',
                align: 'start'
            }
        }
    });
}
function getDataForChart() {
    var startDate = document.getElementById('startDatePicker').value;
    var endDate = document.getElementById('endDatePicker').value;
    var idDv = document.getElementById('idDvDropdown').value;

    $.ajax({
        type: 'GET',
        url: '/ThongKeRecord/UpdateChart',
        data: { startDate: startDate, endDate: endDate, idDv: idDv },
        success: function (chartData) {
            if (chartData && chartData.labels && chartData.dataTongQS && chartData.dataQsVang) {
                drawChart(chartData.labels, chartData.dataTongQS, chartData.dataQsVang);
            } else {
                alert('Dữ liệu không hợp lệ.');
            }
        },
        error: function () {
            alert('Có lỗi xảy ra khi lấy dữ liệu từ máy chủ.');
        }
    });
}

function getDataForChartHV() {
    var startDate = document.getElementById('startDatePicker').value;
    var endDate = document.getElementById('endDatePicker').value;

    $.ajax({
        type: 'GET',
        url: '/ThongKeRecord/UpdateChartForHocVien',
        data: { startDate: startDate, endDate: endDate },
        success: function (chartData) {
            if (chartData && chartData.labels && chartData.dataTongQS && chartData.dataQsVang) {
                drawChart(chartData.labels, chartData.dataTongQS, chartData.dataQsVang);
            } else {
                alert('Dữ liệu không hợp lệ.');
            }
        },
        error: function () {
            alert('Có lỗi xảy ra khi lấy dữ liệu từ máy chủ.');
        }
    });
}

*/
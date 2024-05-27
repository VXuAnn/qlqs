/*var chart1; // Đổi tên biến chart thành chart1
var chart2;

function drawChart1(labels, dataTongQS, dataQsVang) {
    var ctx = document.getElementById('tkchart1').getContext('2d');
    if (chart1) { // Thay chart thành chart1
        chart1.destroy();
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

    chart1 = new Chart(ctx, {
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

function drawChart2(labels, dataTongQS, dataQsVang) {
    var ctx = document.getElementById('tkchart2').getContext('2d');
    if (chart2) {
        chart2.destroy();
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

    chart2 = new Chart(ctx, {
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
                drawChart1(chartData.labels, chartData.dataTongQS, chartData.dataQsVang);
                drawChart2(chartData.labels, chartData.dataTongQS, chartData.dataQsVang);
                
            } else {
                alert('Dữ liệu không hợp lệ.');
            }
        },
        error: function () {
            alert('Có lỗi xảy ra khi lấy dữ liệu từ máy chủ.');
        }
    });
}


var chart;

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



function drawLineChart(labels, dataTongQS) {
    var ctx = document.getElementById('lineChart').getContext('2d');
    var chartData = {
        labels: labels,
        datasets: [{
            label: 'Tổng quân số',
            data: dataTongQS,
            fill: 'rgba(0,119,204,0.3)',
            borderColor: 'rgba(0,119,204,1)',
            borderWidth: 2
        }]
    };
    new Chart(ctx, {
        type: 'line',
        data: chartData,
        options: {
            scales: {
                xAxes: [{
                    display: true
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                        callback: function (value) { return Number(value).toLocaleString(); }
                    }
                }]
            },
            responsive: true,
            legend: {
                display: false
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
                console.log("here");
                drawChart(chartData.labels, chartData.dataTongQS, chartData.dataQsVang);
                drawChart2(chartData.labels, chartData.dataTongQS, chartData.dataQsVang);
            }
        }
    })
}
                
                drawLineChart(chartData.labels, chartData.dataTongQS);
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
            if (chartData && chartData.labels && chartData.dataTongQS) {
                drawLineChart(chartData.labels, chartData.dataTongQS);
            } else {
                alert('Dữ liệu không hợp lệ.');
            }
        },
        error: function () {
            alert('Có lỗi xảy ra khi lấy dữ liệu từ máy chủ.');
        }
    });
}




var chart;

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

}*/
/*
 * Author: Abdullah A Almsaeed
 * Date: 4 Jan 2014
 * Description:
 *      This is a demo file used only for the main dashboard (index.html)
 **/

var $das = jQuery.noConflict();

$(function() {
    "use strict";

    //Make the dashboard widgets sortable Using jquery UI
    $(".connectedSortable").sortable({
        placeholder: "sort-highlight",
        connectWith: ".connectedSortable",
        handle: ".box-header, .nav-tabs",
        forcePlaceholderSize: true,
        zIndex: 999999,
        update: function (event, ui) {
            var ids = [];
            $('.item_dasboard').each(function (index, value) {
                var id = $(value).prop('id');
                ids.push(id);
            });
            $.post(
                '/AdminDasboard/dasboardSiralamDegistir',
                { Ids: ids },
                function (response) {
                }
            );
        }
    }).disableSelection();
    $(".connectedSortable .box-header, .connectedSortable .nav-tabs-custom").css("cursor", "move");

    //jQuery UI sortable for the todo list
    $(".todo-list").sortable({

        placeholder: "sort-highlight",
        handle: ".handle",
        forcePlaceholderSize: true,
        zIndex: 999999,
        update: function (event, ui) {
            var ids = [];
            $('.list_item').each(function (index, value) {
                var id = $(value).prop('id');
                ids.push(id);
            });
            $.post(
                '/AdminDasboard/yapilacakSiralamDegistir',
                { Ids: ids },
                function (response) {
                }
            );
        }
    }).disableSelection();


    //bootstrap WYSIHTML5 - text editor
    //$das(".textarea").wysihtml5();

    $das('.daterange').daterangepicker(
            {
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
                    'Last 7 Days': [moment().subtract('days', 6), moment()],
                    'Last 30 Days': [moment().subtract('days', 29), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
                },
                startDate: moment().subtract('days', 29),
                endDate: moment()
            },
    function(start, end) {
        alert("You chose: " + start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    });

    /* jQueryKnob */
    //$das(".knob").knob();

        
    //Sparkline charts
    var myvalues = [1000, 1200, 920, 927, 931, 1027, 819, 930, 1021];
    $das('#sparkline-1').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });
    myvalues = [515, 519, 520, 522, 652, 810, 370, 627, 319, 630, 921];
    $das('#sparkline-2').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });
    myvalues = [15, 19, 20, 22, 33, 27, 31, 27, 19, 30, 21];
    $das('#sparkline-3').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });    

    //The Calender
    $das("#calendar").datepicker();

    //SLIMSCROLL FOR CHAT WIDGET
    //$das('#chat-box').slimScroll({
    //    height: '250px'
    //});
 
    //Fix for charts under tabs
    $('.box ul.nav a').on('shown.bs.tab', function(e) {
        area.redraw();
        donut.redraw();
    });

    
    /* BOX REFRESH PLUGIN EXAMPLE (usage with morris charts) */
    $("#loading-example").boxRefresh({
        source: "ajax/dashboard-boxrefresh-demo.php",
        onLoadDone: function(box) {
            bar = new Morris.Bar({
                element: 'bar-chart',
                resize: true,
                data: [
                    {y: '2006', a: 100, b: 90},
                    {y: '2007', a: 75, b: 65},
                    {y: '2008', a: 50, b: 40},
                    {y: '2009', a: 75, b: 65},
                    {y: '2010', a: 50, b: 40},
                    {y: '2011', a: 75, b: 65},
                    {y: '2012', a: 100, b: 90}
                ],
                barColors: ['#00a65a', '#f56954'],
                xkey: 'y',
                ykeys: ['a', 'b'],
                labels: ['CPU', 'DISK'],
                hideHover: 'auto'
            });
        }
    });

    /* The todo list plugin */
    $(".todo-list").todolist({
        onCheck: function(ele) {
            //console.log("The element has been checked")
        },
        onUncheck: function(ele) {
            //console.log("The element has been unchecked")
        }
    });

});
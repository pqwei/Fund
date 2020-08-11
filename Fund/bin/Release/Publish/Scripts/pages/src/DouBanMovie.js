require(["../../components/config"], function () {
    require(['jquery', 'AJAX', 'select2', 'doT', 'ajaxCheckModel', 'Handsontable', 'jquery-ui', 'fundComponent', 'timepicker', 'datepicker-zh-CN', 'BlockUI'],
        function ($, AJAX, select2, doT, ajaxCheckModel, Handsontable, datepicker, datetimepicker, blockUI, unblockUI) {
            $(document).ready(function () {
                getMovieData();
            });
            //电影详情
            $(document).on('click', '.movieDetail', function () {
                var movieId = parseInt($(this).prop("name"));
                location.href = "/DouBanMovie/MovieDetail?MovieId=" + movieId;
            });
            //加载电影数据
            function getMovieData() {
                var currentPage = 1;
                var page=JSON.parse(sessionStorage.getItem('page'));
                if (page != null) {
                    currentPage = page;
                };
                var pageHook = "moviePageHook";
                link = "/DouBanMovie/MovieList";
                GetListParameter = {
                    currentPage: currentPage
                };
                drawDoTable(GetListParameter, link).then(function (res) {
                    drawDoTPaginator(res, pageHook);
                })
            };
            //抓取影评
            $(document).on('click', '#MovieReviewGrab', function () {
                var movieId = parseInt($(this).prop("name"));
                location.href = "/DouBanMovie/MovieDetail?MovieId=" + movieId;
            });
        })
})
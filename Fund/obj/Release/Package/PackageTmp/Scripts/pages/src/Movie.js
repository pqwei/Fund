var vm = new Vue({
    el: '#vueMessage',
    data: {
        favors: [],
    },
    methods: {
        get: function () {
            var _self = this;
            _self.favors = [];
            this.$http.get("/douban/pager").then(function (res) {
                for (var i = 0; i < res.body.length; i++) {
                    _self.favors.push(res.body[i]);
                }
            });
        },
        handleSizeChange(val) {
            //console.log(`每页 ${val} 条`);
            this.get();
        },
        handleCurrentChange(val) {
            this.get();
        }

    }
});
vm.get();
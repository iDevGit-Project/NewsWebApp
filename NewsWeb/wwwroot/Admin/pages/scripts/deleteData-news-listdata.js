﻿(function ($) {
    function NewsData() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-news").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new NewsData();
        self.init();
    })
}(jQuery))

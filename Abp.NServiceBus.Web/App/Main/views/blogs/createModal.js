(function () {
    angular.module('app').controller('app.views.blogs.createModal', [
        '$scope', '$modalInstance', 'abp.services.app.blog', 'blogId',
        function ($scope, $modalInstance, blogService, blogId) {
            var vm = this;

            vm.newBlog = (!blogId);
            vm.blog = {};

            vm.save = function () {
                if (vm.newBlog) {
                    blogService.createBlog(vm.blog)
                        .success(function () {
                            abp.notify.info(App.localize('SavedSuccessfully'));
                            $modalInstance.close();
                        });
                } else {
                    blogService.updateBlog(vm.blog)
                        .success(function () {
                            abp.notify.info(App.localize('SavedSuccessfully'));
                            $modalInstance.close();
                        });
                }
            };

            vm.cancel = function () {
                $modalInstance.dismiss();
            };

            function init() {
                if (!vm.newBlog) {
                    blogService.getBlog({ id: blogId })
                        .success(function (result) {
                            vm.blog = result;
                        });
                }
            }

            init();
        }
    ]);
})();
(function() {
    angular.module('app').controller('app.views.blogs.index', [
        '$scope', '$modal', 'abp.services.app.blog',
        function ($scope, $modal, blogService) {
            var vm = this;

            vm.blogs = [];

            function getBlogs() {
                blogService.getBlogs({}).success(function (result) {
                    vm.blogs = result.items;
                });
            }

            vm.deleteBlog = function (blog) {
                abp.message.confirm(
                    App.localize('AreYouSureToDeleteBlog', blog.name),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            blogService.deleteBlog({
                                id: blog.id
                            }).success(function () {
                                abp.notify.success(App.localize('SuccessfullyDeleted'));
                                vm.getBlogs();
                            });
                        }
                    }
                );
            };

            vm.openBlogCreationModal = function (blog) {

                var blogId = null;

                if (blog)
                    blogId = blog.id;

                var modalInstance = $modal.open({
                    templateUrl: '/App/Main/views/blogs/createModal.cshtml',
                    controller: 'app.views.blogs.createModal as vm',
                    backdrop: 'static',
                    resolve: {
                        blogId: function () {
                            return blogId;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    getBlogs();
                });
            };

            getBlogs();
        }
    ]);
})();
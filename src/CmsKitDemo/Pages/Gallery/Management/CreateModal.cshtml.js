abp.modals.ImageManagementCreate = function () {

    function initModal(modalManager, args) {
        var fileUploadUri = "/api/cms-kit-admin/media/page";

        var fileInput = document.getElementById("ImageFile");
        var file;

        fileInput.addEventListener('change', function () {
            abp.ui.block();

            file = fileInput.files[0];

            if (file === undefined) {
                $("#ImageFile").val('');
                return;
            }

            var permittedExtensions = ["jpg", "jpeg", "png"]
            var fileExtension = $(this).val().split('.').pop();
            if (permittedExtensions.indexOf(fileExtension) === -1) {
                abp.message.error(l('ThisExtensionIsNotAllowed'))
                    .then(() => {
                        $("#ImageFile").val('');
                        file = null;
                    });
            }
            else if (file.size > 1024 * 1024) {
                abp.message.error(l('TheFileIsTooBig'))
                    .then(() => {
                        $('#ImageFile').val('');
                        file = null;
                    });
                return;
            }

            uploadImage();

            abp.ui.unblock();
        });

        function resizeImage(file, size) {
            return new Promise((resolve, reject) => {
                const image = new Image();
                image.src = URL.createObjectURL(file);
                image.onload = function() {
                    const canvas = document.createElement('canvas');
                    const context = canvas.getContext('2d');
        
                    const width = image.width;
                    const height = image.height;
        
                    const minSize = Math.min(width, height);
                    const offsetX = width > minSize ? (width - minSize) / 2 : 0;
                    const offsetY = height > minSize ? (height - minSize) / 2 : 0;
        
                    canvas.width = size;
                    canvas.height = size;
        
                    context.drawImage(image, offsetX, offsetY, minSize, minSize, 0, 0, size, size);
        
                    canvas.toBlob(resolve, file.type);
                };
                image.onerror = reject;
            });
        }


        function uploadImage() {
            resizeImage(file, 1024)  // 1:1 ratio
            .then(resizedImage => {
                var formData = new FormData();
                formData.append("name", resizedImage.name);
                formData.append("file", resizedImage);
    
                $.ajax(
                    {
                        url: fileUploadUri,
                        data: formData,
                        processData: false,
                        contentType: false,
                        type: "POST",
                        success: function (data) {
                            $("#Image_CoverImageMediaId").val(data.id);
                        },
                        error: function (err) {
                            abp.message.error(err);
                        }
                    }
                );
            })
            .catch(error => {
                console.error('Error resizing image:', error);
            });

           
        }
    };

    return {
        initModal: initModal
    };
};

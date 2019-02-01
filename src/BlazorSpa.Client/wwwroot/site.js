window.profileFiles = {
    readUploadedFileAsText: function (inputFile) {
        const temporaryFileReader = new FileReader();
        return new Promise((resolve, reject) => {
            temporaryFileReader.onerror = () => {
                temporaryFileReader.abort();
                reject(new DOMException("Problem parsing input file."));
            };
            temporaryFileReader.onload = function (event) {
                resolve(event.target.result);
            };

            temporaryFileReader.readAsDataURL(inputFile.files[0]);
        });
    }
};

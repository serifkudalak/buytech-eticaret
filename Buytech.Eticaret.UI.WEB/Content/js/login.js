let videoContainer = document.getElementById("videoContainer");

let isFaceIDActive = false;

//FaceID için Gereken Kod Bloğu
const localHost = "https://localhost:44399"
const video = document.getElementById('video')
let localStream = null;
let isModelsLoaded = false;
let LabeledFaceDescriptors = null;

//Modellerin Yüklenmesi
Promise.all([
    faceapi.nets.tinyFaceDetector.loadFromUri(`${localHost}/public/models`),
    faceapi.nets.faceLandmark68Net.loadFromUri(`${localHost}/public/models`),
    faceapi.nets.faceRecognitionNet.loadFromUri(`${localHost}/public/models`),
    faceapi.nets.ssdMobilenetv1.loadFromUri(`${localHost}/public/models`),
]).then(initApp);

//initApp
async function initApp() {
    LabeledFaceDescriptors = await loadImages();
}

var return_first = function () {
    var tmp = null;
    $.ajax({
        'async': false,
        'type': "POST",
        'global': false,
        'dataType': 'html',
        'url': "/klasor-isimleri",
        'data': { 'request': "", 'target': 'arrange_url', 'method': 'method_target' },
        'success': function (data) {
            tmp = data;
        }
    });
    return tmp;
}();

function loadImages() {
    const label = JSON.parse(return_first);

    return Promise.all(
        label.map(async label => {
            const descriptions = [];

            const img = await faceapi.fetchImage(`${localHost}/Content/img/profile/${label}/1.jpg`)

            const detections = await faceapi.detectSingleFace(img)
                .withFaceLandmarks()
                .withFaceDescriptor();
            descriptions.push(detections.descriptor);

            return new faceapi.LabeledFaceDescriptors(label, descriptions)
        })
    );
}

function startCamera() {
    navigator.getUserMedia(
        {
            video: {}
        }, stream => {
            localStream = stream;
            video.srcObject = stream;
        }, err => console.log(err)
    );
}

function stopCamera() {
    video.pause();
    video.srcObject = null;
    localStream.getTracks().forEach(track => {
        track.stop();
    });
}

window.addEventListener("load", function () {
    startCamera();
});

video.addEventListener('play', async () => {
    const boxSize = {
        width: video.width,
        height: video.height
    };

    let cameraInterval = setInterval(async () => {
        const detections = await faceapi.detectAllFaces(video, new faceapi.TinyFaceDetectorOptions())
            .withFaceLandmarks().withFaceDescriptors();

        const resizedDetections = faceapi.resizeResults(detections, boxSize);

        const faceMatcher = new faceapi.FaceMatcher(LabeledFaceDescriptors, 1.00);

        const results = resizedDetections.map(d => faceMatcher.findBestMatch(d.descriptor));

        if (results.length > 0 && JSON.parse(return_first).indexOf(results[0].label) > -1) {
            var email = function () {
                var tmp = null;
                $.ajax({
                    'async': false,
                    'type': "POST",
                    'global': false,
                    'dataType': 'html',
                    'url': "/email-bilgileri",
                    'data': { globalNumber: results[0].label },
                    'success': function (data) {
                        tmp = data;
                    }
                });
                return tmp;
            }();

            document.getElementById('email').value = email.replace(/['"]+/g, '');
            // clearInterval(cameraInterval);
        }
    }, 100);
});
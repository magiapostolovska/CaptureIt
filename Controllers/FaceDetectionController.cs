using CaptureIt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Threading.Tasks;

namespace CaptureIt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        private readonly IFaceService _faceService;

        public FaceController(IFaceService faceService)
        {
            _faceService = faceService;
        }

        [HttpPost("detect")]
        public async Task<IActionResult> DetectFace([FromBody] string imageUrl)
        {
            const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;
            var detectedFaces = await _faceService.DetectFaceRecognize(imageUrl, RECOGNITION_MODEL4);
            return Ok(detectedFaces);
        }

        [HttpPost("identify")]
        public async Task<IActionResult> IdentifyFaces([FromBody] string imageUrl)
        {
            const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;
            await _faceService.IdentifyInPersonGroup(imageUrl, RECOGNITION_MODEL4);
            return Ok();
        }
    }
}






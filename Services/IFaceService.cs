using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace CaptureIt.Services
{
    public interface IFaceService
    {
        Task<List<DetectedFace>> DetectFaceRecognize(string url, string recognitionModel);
        Task IdentifyInPersonGroup(string url, string recognitionModel);
    }
}

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CaptureIt.Services
{


    public class FaceService : IFaceService
    {
        private readonly IFaceClient _faceClient;
        private static string personGroupId = Guid.NewGuid().ToString();

        public FaceService(IFaceClient faceClient)
        {
            _faceClient = faceClient;
        }

        public async Task<List<DetectedFace>> DetectFaceRecognize(string imageUrl, string recognitionModel)
        {
            IList<DetectedFace> detectedFaces = await _faceClient.Face.DetectWithUrlAsync(
                imageUrl,
                returnFaceId: true,
                returnFaceLandmarks: true,
                returnFaceAttributes: new List<FaceAttributeType>
                {
            FaceAttributeType.Age,
            FaceAttributeType.Gender,
            FaceAttributeType.Smile,
            FaceAttributeType.FacialHair,
            FaceAttributeType.Glasses,
            FaceAttributeType.HeadPose,
            FaceAttributeType.Emotion,
            FaceAttributeType.Hair,
            FaceAttributeType.Makeup,
            FaceAttributeType.Occlusion,
            FaceAttributeType.Accessories,
            FaceAttributeType.Blur,
            FaceAttributeType.Exposure,
            FaceAttributeType.Noise,
            FaceAttributeType.Mask,
            FaceAttributeType.QualityForRecognition
                },
                recognitionModel: recognitionModel,
                detectionModel: DetectionModel.Detection03);

            List<DetectedFace> sufficientQualityFaces = new List<DetectedFace>();
            foreach (DetectedFace detectedFace in detectedFaces)
            {
                var faceQualityForRecognition = detectedFace.FaceAttributes.QualityForRecognition;
                if (faceQualityForRecognition.HasValue && (faceQualityForRecognition.Value >= QualityForRecognition.Medium))
                {
                    sufficientQualityFaces.Add(detectedFace);
                }
            }
            Console.WriteLine($"{detectedFaces.Count} face(s) with {sufficientQualityFaces.Count} having sufficient quality for recognition detected from image `{Path.GetFileName(imageUrl)}`");

            return sufficientQualityFaces.ToList();
        }


        public async Task IdentifyInPersonGroup(string url, string recognitionModel)
        {
            Dictionary<string, string[]> personDictionary = new Dictionary<string, string[]>
            {
                { "Family1-Dad", new[] { "Family1-Dad1.jpg", "Family1-Dad2.jpg" } },
                { "Family1-Mom", new[] { "Family1-Mom1.jpg", "Family1-Mom2.jpg" } },
                { "Family1-Son", new[] { "Family1-Son1.jpg", "Family1-Son2.jpg" } },
                { "Family1-Daughter", new[] { "Family1-Daughter1.jpg", "Family1-Daughter2.jpg" } },
                { "Family2-Lady", new[] { "Family2-Lady1.jpg", "Family2-Lady2.jpg" } },
                { "Family2-Man", new[] { "Family2-Man1.jpg", "Family2-Man2.jpg" } }
            };

            string sourceImageFileName = "identification1.jpg";

            await _faceClient.PersonGroup.CreateAsync(personGroupId, personGroupId, recognitionModel: recognitionModel);

            foreach (var groupedFace in personDictionary.Keys)
            {
                await Task.Delay(250);
                Person person = await _faceClient.PersonGroupPerson.CreateAsync(personGroupId: personGroupId, name: groupedFace);

                foreach (var similarImage in personDictionary[groupedFace])
                {
                    IList<DetectedFace> detectedFaces1 = await _faceClient.Face.DetectWithUrlAsync($"{url}{similarImage}", recognitionModel: recognitionModel, detectionModel: DetectionModel.Detection03, returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.QualityForRecognition });
                    bool sufficientQuality = true;
                    foreach (var face1 in detectedFaces1)
                    {
                        var faceQualityForRecognition = face1.FaceAttributes.QualityForRecognition;
                        if (faceQualityForRecognition.HasValue && (faceQualityForRecognition.Value != QualityForRecognition.High))
                        {
                            sufficientQuality = false;
                            break;
                        }
                    }

                    if (!sufficientQuality)
                    {
                        continue;
                    }

                    PersistedFace face = await _faceClient.PersonGroupPerson.AddFaceFromUrlAsync(personGroupId, person.PersonId, $"{url}{similarImage}", similarImage);
                }
            }

            await _faceClient.PersonGroup.TrainAsync(personGroupId);

            while (true)
            {
                await Task.Delay(1000);
                var trainingStatus = await _faceClient.PersonGroup.GetTrainingStatusAsync(personGroupId);
                if (trainingStatus.Status == TrainingStatusType.Succeeded) { break; }
            }

            List<Guid> sourceFaceIds = new List<Guid>();
            List<DetectedFace> detectedFaces = await DetectFaceRecognize($"{url}{sourceImageFileName}", recognitionModel);

            foreach (var detectedFace in detectedFaces) { sourceFaceIds.Add(detectedFace.FaceId.Value); }

            var identifyResults = await _faceClient.Face.IdentifyAsync(sourceFaceIds, personGroupId);

            foreach (var identifyResult in identifyResults)
            {
                if (identifyResult.Candidates.Count == 0)
                {
                    continue;
                }
                Person person = await _faceClient.PersonGroupPerson.GetAsync(personGroupId, identifyResult.Candidates[0].PersonId);
                var verifyResult = await _faceClient.Face.VerifyFaceToPersonAsync(identifyResult.FaceId, person.PersonId, personGroupId);
            }
        }
    }
}


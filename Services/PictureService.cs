﻿using AutoMapper;
using CaptureIt.DTOs.Album;
using CaptureIt.DTOs.Picture;
using CaptureIt.Models;
using CaptureIt.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaptureIt.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;

        public PictureService(IPictureRepository pictureRepository, IMapper mapper)
        {
            _pictureRepository = pictureRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PictureResponse>> GetAll(DateTime? createdAt = default, int albumId = default)
        {
            var pictures = await _pictureRepository.GetAll();
            if (createdAt.HasValue)
            {
                pictures = pictures.Where(e => e.CreatedAt.HasValue && e.CreatedAt.Value.Date >= createdAt.Value.Date);
            }
            if (albumId != default)
            {
                pictures = pictures.Where(a => a.AlbumId == albumId);
            }
            return _mapper.Map<IEnumerable<PictureResponse>>(pictures);
        }

            public async Task<PictureResponse> GetById(int id)
        {
            var picture = await _pictureRepository.GetById(id);
            return _mapper.Map<PictureResponse>(picture);
        }

        public async Task<PictureResponse> Add(PictureRequest pictureRequest)
        {
            var picture = _mapper.Map<Picture>(pictureRequest);
            await _pictureRepository.Add(picture);
            return _mapper.Map<PictureResponse>(picture);
        }

        public async Task<PictureResponse> Update(int id, PictureUpdate pictureUpdate)
        {
            var picture = await _pictureRepository.GetById(id);
            if (picture == null)
            {
                return null;
            }

            _mapper.Map(pictureUpdate, picture);

            await _pictureRepository.Update(picture);
            return _mapper.Map<PictureResponse>(picture);
        }

        public async Task<bool> Delete(int id)
        {
            var picture = await _pictureRepository.GetById(id);
            if (picture == null)
            {
                return false;
            }

            await _pictureRepository.Delete(id);
            return true;
        }

        public async Task<PictureResponse> UpdateLikeCount(int id, PictureLikes pictureUpdate)
        {
            var picture = await _pictureRepository.GetById(id);
            if (picture == null)
            {
                return null;
            }

            _mapper.Map(pictureUpdate, picture);

            await _pictureRepository.Update(picture);
            return _mapper.Map<PictureResponse>(picture);
        }
        public async Task<PictureResponse> UpdateCommentCount(int id, PictureComments pictureUpdate)
        {
            var picture = await _pictureRepository.GetById(id);
            if (picture == null)
            {
                return null;
            }

            _mapper.Map(pictureUpdate, picture);

            await _pictureRepository.Update(picture);
            return _mapper.Map<PictureResponse>(picture);
        }

        public async Task<int> GetNumberOfPhotos(int albumId)
        {
            return await _pictureRepository.GetNumberOfPhotos(albumId);
        }
        public async Task<PictureResponse> Update(int id, PictureAuthor pictureUpdate)
        {
            var picture = await _pictureRepository.GetById(id);
            if (picture == null)
            {
                return null;
            }

            _mapper.Map(pictureUpdate, picture);

            await _pictureRepository.Update(picture);
            return _mapper.Map<PictureResponse>(picture);
        }
    }
}


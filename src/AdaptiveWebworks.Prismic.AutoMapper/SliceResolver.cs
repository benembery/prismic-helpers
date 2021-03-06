using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using prismic;
using prismic.fragments;

namespace AdaptiveWebworks.Prismic.AutoMapper
{
    public class SliceResolver<TDest> : IValueResolver<Document, TDest, IList<IMappedSlice>>
    {
        private readonly string _fieldName;

        public SliceResolver(string fieldName)
        {
            _fieldName = fieldName;
        }

        public IList<IMappedSlice> Resolve(Document source, TDest destination, IList<IMappedSlice> destMember, ResolutionContext context)
        {
            var mappedMember = destMember ?? new List<IMappedSlice>();

            var slices = GetSlices(source);

            if (slices == null || !slices.Any())
                return mappedMember;

            foreach (var slice in slices)
            {
                var mappedSlice = MapSliceType(source, slice, context);

                if (mappedSlice != null)
                    mappedMember.Add(mappedSlice);
            }

            return mappedMember;
        }

        private IList<CompositeSlice> GetSlices(Document document)
        {
            var sliceZone = document.GetSliceZone($"{document.Type}.{_fieldName}");

            return sliceZone?.Slices.OfType<CompositeSlice>().ToList();
        }

        protected virtual IMappedSlice MapSliceType(Document document, CompositeSlice slice, ResolutionContext context)
        {
            return null;
        }
    }
}

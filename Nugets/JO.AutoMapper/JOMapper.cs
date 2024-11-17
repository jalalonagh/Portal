using AutoMapper;

namespace JO.AutoMapper
{
    public class JOMapper : IJOMapper
    {
        private IMapper mapper;

        public JOMapper(IMapper _mapper)
        {
            mapper = _mapper;
        }

        public TDS? Map<TDS>(object? src)
        {
            if (src == null)
                return default;

            return mapper.Map<TDS>(src);
        }
    }
}

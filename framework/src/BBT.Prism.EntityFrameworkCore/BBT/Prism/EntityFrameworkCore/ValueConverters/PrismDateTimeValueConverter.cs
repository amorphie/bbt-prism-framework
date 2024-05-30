using System;
using BBT.Prism.Timing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BBT.Prism.EntityFrameworkCore.ValueConverters;

public class PrismDateTimeValueConverter(IClock clock, ConverterMappingHints? mappingHints = null)
    : ValueConverter<DateTime, DateTime>(x => clock.Normalize(x),
        x => clock.Normalize(x), mappingHints);

public class PrismNullableDateTimeValueConverter(IClock clock, ConverterMappingHints? mappingHints = null)
    : ValueConverter<DateTime?, DateTime?>(x => x.HasValue ? clock.Normalize(x.Value) : x,
        x => x.HasValue ? clock.Normalize(x.Value) : x, mappingHints);
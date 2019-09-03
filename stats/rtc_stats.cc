/*
 *  Copyright 2016 The WebRTC Project Authors. All rights reserved.
 *
 *  Use of this source code is governed by a BSD-style license
 *  that can be found in the LICENSE file in the root of the source
 *  tree. An additional intellectual property rights grant can be found
 *  in the file PATENTS.  All contributing project authors may
 *  be found in the AUTHORS file in the root of the source tree.
 */

#include "api/stats/rtc_stats.h"

#include <cstdio>

#include "rtc_base/arraysize.h"
#include "rtc_base/string_encode.h"
#include "rtc_base/strings/string_builder.h"

namespace webrtc {

namespace {

// Produces "[a,b,c]". Works for non-vector |RTCStatsMemberInterface::Type|
// types.
template <typename T>
std::string VectorToString(const std::vector<T>& vector) {
  rtc::StringBuilder sb;
  sb << "[";
  const char* separator = "";
  for (const T& element : vector) {
    sb << separator << rtc::ToString(element);
    separator = ",";
  }
  sb << "]";
  return sb.Release();
}

// Produces "[\"a\",\"b\",\"c\"]". Works for vectors of both const char* and
// std::string element types.
template <typename T>
std::string VectorOfStringsToString(const std::vector<T>& strings) {
  rtc::StringBuilder sb;
  sb << "[";
  const char* separator = "";
  for (const T& element : strings) {
    sb << separator << "\"" << rtc::ToString(element) << "\"";
    separator = ",";
  }
  sb << "]";
  return sb.Release();
}

template <typename T>
std::string ToStringAsDouble(const T value) {
  // JSON represents numbers as floating point numbers with about 15 decimal
  // digits of precision.
  char buf[32];
  const int len = std::snprintf(&buf[0], arraysize(buf), "%.16g",
                                static_cast<double>(value));
  RTC_DCHECK_LE(len, arraysize(buf));
  return std::string(&buf[0], len);
}

template <typename T>
std::string VectorToStringAsDouble(const std::vector<T>& vector) {
  rtc::StringBuilder sb;
  sb << "[";
  const char* separator = "";
  for (const T& element : vector) {
    sb << separator << ToStringAsDouble<T>(element);
    separator = ",";
  }
  sb << "]";
  return sb.Release();
}

}  // namespace

bool RTCStats::operator==(const RTCStats& other) const {
  if (type() != other.type() || id() != other.id())
    return false;
  std::vector<const RTCStatsMemberInterface*> members = Members();
  std::vector<const RTCStatsMemberInterface*> other_members = other.Members();
  RTC_DCHECK_EQ(members.size(), other_members.size());
  for (size_t i = 0; i < members.size(); ++i) {
    const RTCStatsMemberInterface* member = members[i];
    const RTCStatsMemberInterface* other_member = other_members[i];
    RTC_DCHECK_EQ(member->type(), other_member->type());
    RTC_DCHECK_EQ(member->name(), other_member->name());
    if (*member != *other_member)
      return false;
  }
  return true;
}

bool RTCStats::operator!=(const RTCStats& other) const {
  return !(*this == other);
}

std::string RTCStats::ToJson() const {
  rtc::StringBuilder sb;
  sb << "{\"type\":\"" << type() << "\","
     << "\"id\":\"" << id_ << "\","
     << "\"timestamp\":" << timestamp_us_;
  for (const RTCStatsMemberInterface* member : Members()) {
    if (member->is_defined()) {
      sb << ",\"" << member->name() << "\":";
      if (member->is_string())
        sb << "\"" << member->ValueToJson() << "\"";
      else
        sb << member->ValueToJson();
    }
  }
  sb << "}";
  return sb.Release();
}

std::vector<const RTCStatsMemberInterface*> RTCStats::Members() const {
  return MembersOfThisObjectAndAncestors(0);
}

std::vector<const RTCStatsMemberInterface*>
RTCStats::MembersOfThisObjectAndAncestors(size_t additional_capacity) const {
  std::vector<const RTCStatsMemberInterface*> members;
  members.reserve(additional_capacity);
  return members;
}

#define WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(T, is_seq, is_str, to_str, to_json)  \
  template <>                                                                  \
  bool RTCStatsMember<T>::is_sequence() const {                                \
    return is_seq;                                                             \
  }                                                                            \
  template <>                                                                  \
  bool RTCStatsMember<T>::is_string() const {                                  \
    return is_str;                                                             \
  }                                                                            \
  template <>                                                                  \
  std::string RTCStatsMember<T>::ValueToString() const {                       \
    RTC_DCHECK(is_defined_);                                                   \
    return to_str;                                                             \
  }                                                                            \
  template <>                                                                  \
  std::string RTCStatsMember<T>::ValueToJson() const {                         \
    RTC_DCHECK(is_defined_);                                                   \
    return to_json;                                                            \
  }

WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(bool,
                                  false,
                                  false,
                                  rtc::ToString(value_),
                                  rtc::ToString(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(int32_t,
                                  false,
                                  false,
                                  rtc::ToString(value_),
                                  rtc::ToString(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(uint32_t,
                                  false,
                                  false,
                                  rtc::ToString(value_),
                                  rtc::ToString(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(int64_t,
                                  false,
                                  false,
                                  rtc::ToString(value_),
                                  ToStringAsDouble(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(uint64_t,
                                  false,
                                  false,
                                  rtc::ToString(value_),
                                  ToStringAsDouble(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(double,
                                  false,
                                  false,
                                  rtc::ToString(value_),
                                  ToStringAsDouble(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(std::string, false, true, value_, value_)
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(std::vector<bool>,
                                  true,
                                  false,
                                  VectorToString(value_),
                                  VectorToString(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(std::vector<int32_t>,
                                  true,
                                  false,
                                  VectorToString(value_),
                                  VectorToString(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(std::vector<uint32_t>,
                                  true,
                                  false,
                                  VectorToString(value_),
                                  VectorToString(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(std::vector<int64_t>,
                                  true,
                                  false,
                                  VectorToString(value_),
                                  VectorToStringAsDouble(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(std::vector<uint64_t>,
                                  true,
                                  false,
                                  VectorToString(value_),
                                  VectorToStringAsDouble(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(std::vector<double>,
                                  true,
                                  false,
                                  VectorToString(value_),
                                  VectorToStringAsDouble(value_))
WEBRTC_DEFINE_RTCSTATSMEMBER_IMPL(std::vector<std::string>,
                                  true,
                                  false,
                                  VectorOfStringsToString(value_),
                                  VectorOfStringsToString(value_))

}  // namespace webrtc

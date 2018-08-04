// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: echo_client_server.proto

#ifndef PROTOBUF_echo_5fclient_5fserver_2eproto__INCLUDED
#define PROTOBUF_echo_5fclient_5fserver_2eproto__INCLUDED

#include <string>

#include <google/protobuf/stubs/common.h>

#if GOOGLE_PROTOBUF_VERSION < 3003000
#error This file was generated by a newer version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please update
#error your headers.
#endif
#if 3003000 < GOOGLE_PROTOBUF_MIN_PROTOC_VERSION
#error This file was generated by an older version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please
#error regenerate this file with a newer version of protoc.
#endif

#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/arena.h>
#include <google/protobuf/arenastring.h>
#include <google/protobuf/generated_message_table_driven.h>
#include <google/protobuf/generated_message_util.h>
#include <google/protobuf/metadata.h>
#include <google/protobuf/message.h>
#include <google/protobuf/repeated_field.h>  // IWYU pragma: export
#include <google/protobuf/extension_set.h>  // IWYU pragma: export
#include <google/protobuf/generated_enum_reflection.h>
#include <google/protobuf/unknown_field_set.h>
#include "echo_common.pb.h"  // IWYU pragma: export
// @@protoc_insertion_point(includes)
namespace pb_echo {
namespace PC2S {
class Chat;
class ChatDefaultTypeInternal;
extern ChatDefaultTypeInternal _Chat_default_instance_;
}  // namespace PC2S
}  // namespace pb_echo

namespace pb_echo {
namespace PC2S {

namespace protobuf_echo_5fclient_5fserver_2eproto {
// Internal implementation detail -- do not call these.
struct TableStruct {
  static const ::google::protobuf::internal::ParseTableField entries[];
  static const ::google::protobuf::internal::AuxillaryParseTableField aux[];
  static const ::google::protobuf::internal::ParseTable schema[];
  static const ::google::protobuf::uint32 offsets[];
  static void InitDefaultsImpl();
  static void Shutdown();
};
void AddDescriptors();
void InitDefaults();
}  // namespace protobuf_echo_5fclient_5fserver_2eproto

enum ProtocolNumber {
  EChat = 0,
  ProtocolNumber_INT_MIN_SENTINEL_DO_NOT_USE_ = ::google::protobuf::kint32min,
  ProtocolNumber_INT_MAX_SENTINEL_DO_NOT_USE_ = ::google::protobuf::kint32max
};
bool ProtocolNumber_IsValid(int value);
const ProtocolNumber ProtocolNumber_MIN = EChat;
const ProtocolNumber ProtocolNumber_MAX = EChat;
const int ProtocolNumber_ARRAYSIZE = ProtocolNumber_MAX + 1;

const ::google::protobuf::EnumDescriptor* ProtocolNumber_descriptor();
inline const ::std::string& ProtocolNumber_Name(ProtocolNumber value) {
  return ::google::protobuf::internal::NameOfEnum(
    ProtocolNumber_descriptor(), value);
}
inline bool ProtocolNumber_Parse(
    const ::std::string& name, ProtocolNumber* value) {
  return ::google::protobuf::internal::ParseNamedEnum<ProtocolNumber>(
    ProtocolNumber_descriptor(), name, value);
}
// ===================================================================

class Chat : public ::google::protobuf::Message /* @@protoc_insertion_point(class_definition:pb_echo.PC2S.Chat) */ {
 public:
  Chat();
  virtual ~Chat();

  Chat(const Chat& from);

  inline Chat& operator=(const Chat& from) {
    CopyFrom(from);
    return *this;
  }

  static const ::google::protobuf::Descriptor* descriptor();
  static const Chat& default_instance();

  static inline const Chat* internal_default_instance() {
    return reinterpret_cast<const Chat*>(
               &_Chat_default_instance_);
  }
  static PROTOBUF_CONSTEXPR int const kIndexInFileMessages =
    0;

  void Swap(Chat* other);

  // implements Message ----------------------------------------------

  inline Chat* New() const PROTOBUF_FINAL { return New(NULL); }

  Chat* New(::google::protobuf::Arena* arena) const PROTOBUF_FINAL;
  void CopyFrom(const ::google::protobuf::Message& from) PROTOBUF_FINAL;
  void MergeFrom(const ::google::protobuf::Message& from) PROTOBUF_FINAL;
  void CopyFrom(const Chat& from);
  void MergeFrom(const Chat& from);
  void Clear() PROTOBUF_FINAL;
  bool IsInitialized() const PROTOBUF_FINAL;

  size_t ByteSizeLong() const PROTOBUF_FINAL;
  bool MergePartialFromCodedStream(
      ::google::protobuf::io::CodedInputStream* input) PROTOBUF_FINAL;
  void SerializeWithCachedSizes(
      ::google::protobuf::io::CodedOutputStream* output) const PROTOBUF_FINAL;
  ::google::protobuf::uint8* InternalSerializeWithCachedSizesToArray(
      bool deterministic, ::google::protobuf::uint8* target) const PROTOBUF_FINAL;
  int GetCachedSize() const PROTOBUF_FINAL { return _cached_size_; }
  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const PROTOBUF_FINAL;
  void InternalSwap(Chat* other);
  private:
  inline ::google::protobuf::Arena* GetArenaNoVirtual() const {
    return NULL;
  }
  inline void* MaybeArenaPtr() const {
    return NULL;
  }
  public:

  ::google::protobuf::Metadata GetMetadata() const PROTOBUF_FINAL;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  // string message = 1;
  void clear_message();
  static const int kMessageFieldNumber = 1;
  const ::std::string& message() const;
  void set_message(const ::std::string& value);
  #if LANG_CXX11
  void set_message(::std::string&& value);
  #endif
  void set_message(const char* value);
  void set_message(const char* value, size_t size);
  ::std::string* mutable_message();
  ::std::string* release_message();
  void set_allocated_message(::std::string* message);

  // @@protoc_insertion_point(class_scope:pb_echo.PC2S.Chat)
 private:

  ::google::protobuf::internal::InternalMetadataWithArena _internal_metadata_;
  ::google::protobuf::internal::ArenaStringPtr message_;
  mutable int _cached_size_;
  friend struct protobuf_echo_5fclient_5fserver_2eproto::TableStruct;
};
// ===================================================================


// ===================================================================

#if !PROTOBUF_INLINE_NOT_IN_HEADERS
// Chat

// string message = 1;
inline void Chat::clear_message() {
  message_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline const ::std::string& Chat::message() const {
  // @@protoc_insertion_point(field_get:pb_echo.PC2S.Chat.message)
  return message_.GetNoArena();
}
inline void Chat::set_message(const ::std::string& value) {
  
  message_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), value);
  // @@protoc_insertion_point(field_set:pb_echo.PC2S.Chat.message)
}
#if LANG_CXX11
inline void Chat::set_message(::std::string&& value) {
  
  message_.SetNoArena(
    &::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::move(value));
  // @@protoc_insertion_point(field_set_rvalue:pb_echo.PC2S.Chat.message)
}
#endif
inline void Chat::set_message(const char* value) {
  GOOGLE_DCHECK(value != NULL);
  
  message_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), ::std::string(value));
  // @@protoc_insertion_point(field_set_char:pb_echo.PC2S.Chat.message)
}
inline void Chat::set_message(const char* value, size_t size) {
  
  message_.SetNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(),
      ::std::string(reinterpret_cast<const char*>(value), size));
  // @@protoc_insertion_point(field_set_pointer:pb_echo.PC2S.Chat.message)
}
inline ::std::string* Chat::mutable_message() {
  
  // @@protoc_insertion_point(field_mutable:pb_echo.PC2S.Chat.message)
  return message_.MutableNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline ::std::string* Chat::release_message() {
  // @@protoc_insertion_point(field_release:pb_echo.PC2S.Chat.message)
  
  return message_.ReleaseNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}
inline void Chat::set_allocated_message(::std::string* message) {
  if (message != NULL) {
    
  } else {
    
  }
  message_.SetAllocatedNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), message);
  // @@protoc_insertion_point(field_set_allocated:pb_echo.PC2S.Chat.message)
}

#endif  // !PROTOBUF_INLINE_NOT_IN_HEADERS

// @@protoc_insertion_point(namespace_scope)


}  // namespace PC2S
}  // namespace pb_echo

#ifndef SWIG
namespace google {
namespace protobuf {

template <> struct is_proto_enum< ::pb_echo::PC2S::ProtocolNumber> : ::google::protobuf::internal::true_type {};
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::pb_echo::PC2S::ProtocolNumber>() {
  return ::pb_echo::PC2S::ProtocolNumber_descriptor();
}

}  // namespace protobuf
}  // namespace google
#endif  // SWIG

// @@protoc_insertion_point(global_scope)

#endif  // PROTOBUF_echo_5fclient_5fserver_2eproto__INCLUDED
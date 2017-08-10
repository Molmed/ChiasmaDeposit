using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.ChiasmaDep.Database
{
    public struct BarCodeData
    {
        public const String BAR_CODE = "barcode";
        public const String BAR_CODE_COLUMN = "code";
        public const String CODE_LENGTH = "code_length";
        public const String IDENTIFIABLE_ID = "identifiable_id";
        public const String KIND = "kind";
        public const String TABLE_EXTERNAL = "external_barcode";
        public const String TABLE_INTERNAL = "internal_barcode";
    }

    public struct ContainerData
    {
        public const String ACTIVE_CONTAINERS = "active_containers";
        public const String BAR_CODE = "barcode";
        public const String BAR_CODE_LENGTH = "barcode_length";
        public const String COMMENT = "comment";
        public const String CONTAINER_ID = "id";
        public const String EXTERNAL_BAR_CODE = "external_barcode";
        public const String IDENTIFIABLE_ID = "identifiable_id";
        public const String IDENTIFIER = "identifier";
        public const String IDENTIFIER_FILTER = "identifier_filter";
        public const String SIZE_X = "size_x";
        public const String SIZE_Y = "size_y";
        public const String SIZE_Z = "size_z";
        public const String STATUS = "status";
        public const String STATUS_CHANGED = "status_changed";
        public const String TABLE = "container";
        public const String TO_CONTAINER_ID = "to_container_id";
        public const String TYPE = "type";
        public const String AUTHORITY_ID = "authority_id";
    }

    public struct DataCommentData
    {
        public const String COMMENT = "comment";
    }

    public struct DataIdentifierData
    {
        public const String IDENTIFIER = "identifier";
    }
    public struct DataIdentityData
    {
        public const String ID = "id";
    }

    public struct GenericContainerData
    {
        public const String ACTIVE_CONTAINERS = "active_containers";
        public const String BAR_CODE = "barcode";
        public const String COMMENT = "comment";
        public const String GENERIC_CONTAINER_ID = "id";
        public const String IDENTIFIER = "identifier";
        public const String IDENTIFIER_FILTER = "identifier_filter";
        public const String STATUS = "status";
        public const String TABLE = "all_containers";
    }

    public struct LoginData
    {
        public const String CHIASMA_BARCODE = "barcode";
    }

    public struct TubeRackTypeData
    {
        public const String IDENTIFIER = "identifier";
        public const String SIZE_X = "size_x";
        public const String SIZE_Y = "size_y";
        public const String TABLE = "tube_rack_type";
        public const String TUBE_RACK_TYPE_ID = "id";
    }

    public struct TubeRackLabelData
    {
        public const String IDENTIFIER = "identifier";
        public const String POSITION_X = "position_x";
        public const String POSITION_Y = "position_y";
        public const String TABLE = "tube_rack_label";
        public const String TUBE_RACK_TYPE_ID = "tube_rack_type_id";
    }

    public struct TubeRackData
    {
        public const String IDENTIFIER = "identifier";
        public const String TUBE_RACK_TYPE_ID = "tube_rack_type_id";
        public const String TUBE_RACK_ID = "id";
        public const String EMPTY_SLOTS = "empty_slots";
        public const String TUBE_RACK_NUMBER = "tube_rack_number";
        public const String COMMENT = "comment";
        public const String STATUS = "status";
        public const String BARCODE_LENGTH = "barcode_length";
    }

    public struct UserData
    {
        public const String ACCOUNT_STATUS = "account_status";
        public const String COMMENT = "comment";
        public const String IDENTIFIER = "identifier";
        public const String NAME = "name";
        public const String TABLE = "authority";
        public const String USER_ID = "id";
        public const String USER_TYPE = "user_type";
        public const String BAR_CODE = "barcode";
    }

}

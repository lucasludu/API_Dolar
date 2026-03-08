namespace Application.Constants
{
    public static class ApiMessages
    {
        private static readonly string NotFound = "El recurso solicitado no se ha encontrado en el servidor.";
        private static readonly string InternalError = "Se ha producido un error interno. Por favor, inténtelo de nuevo más tarde.";
        private static readonly string BadRequestMessage = "Se ha producido un error de validación.";
        private static readonly string ValidationErrorMessage = "La solicitud es incorrecta o está mal formada.";
        private static readonly string SuccessResponse = "Respuesta exitosa.";
        private static readonly string NotFoundWithID = "Registro no encontrado con el id ";
        private static readonly string DeleteSuccessMessage = "Registro eliminado correctamente.";
        private static readonly string repeatedResource = "Registro repetido";
        private static readonly string UpdatedResource = "Registro Actualizado con exito";
        private static readonly string ExceptionErrorMessage = "Se ha producido un error. Detalles: {0}";
        private static readonly string Created = "Registro creado con exito";

        public static string ValidationError() => ValidationErrorMessage;
        public static string Success() => SuccessResponse;
        public static string NotFoundMessage() => NotFound;
        public static string NotFoundWithIdMessage(int id) => NotFoundWithID + $"{id}.";
        public static string InternalServerError() => InternalError;
        public static string BadRequest() => BadRequestMessage;
        public static string DeleteSuccess() => DeleteSuccessMessage;
        public static string RepeatedResource() => repeatedResource;
        public static string UpdatedSuccess() => UpdatedResource;
        public static string CreatedSuccess() => Created;

        public static string ErrorExceptionMessage(Exception ex)
        {
            return string.Format(ExceptionErrorMessage, ex.Message);
        }



    }
}

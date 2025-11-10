import moment from "moment-timezone";

export const formatDateTime = (dateStr: any) => {
  if (!dateStr) return "";
  return moment(dateStr)
    .tz("Asia/Bangkok")
    .format("DD MMM YYYY - hh:mm A");
};
